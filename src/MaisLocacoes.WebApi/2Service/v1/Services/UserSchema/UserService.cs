using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using Service.v1.IServices.UserSchema;
using System.Net;

namespace Service.v1.Services.UserSchema
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
        }

        public async Task<UserResponse> CreateUser(UserRequest userRequest)
        {
            var existsUser = await _userRepository.UserExists(userRequest.Email, userRequest.Cpf, userRequest.Cnpj);
            if (existsUser)
                throw new HttpRequestException("Cpf ou e-mail de usuário já cadastrado para essa empresa", null, HttpStatusCode.BadRequest);

            var existsCompany = await _companyRepository.CompanyExists(userRequest.Cnpj);
            if (!existsCompany)
                throw new HttpRequestException("Não existe nenhuma empresa cadastrada com esse CNPJ", null, HttpStatusCode.BadRequest);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            userEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            userEntity = await _userRepository.CreateUser(userEntity);

            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<UserResponse> GetByEmail(string email, string cnpj)
        {
            var userEntity = await _userRepository.GetByEmail(email, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<UserResponse>(userEntity);

            return userResponse;
        }

        public async Task<UserResponse> GetByCpf(string cpf, string cnpj)
        {
            var userEntity = await _userRepository.GetByCpf(cpf, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<UserResponse>(userEntity);

            return userResponse;
        }

        public async Task<bool> UpdateUser(UserRequest userRequest, string email, string cnpj)
        {
            var userForUpdate = await _userRepository.GetByEmail(email, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            if (userRequest.Cnpj != userForUpdate.Cnpj)
            {
                var existsCompany = await _companyRepository.CompanyExists(userRequest.Cnpj);
                if (!existsCompany)
                    throw new HttpRequestException("Não existe nenhuma empresa cadastrada com esse CNPJ", null, HttpStatusCode.BadRequest);
            }

            if (userRequest.Cpf != userForUpdate.Cpf)
            {
                var existsCpf = await _userRepository.GetByCpf(userRequest.Cpf, userRequest.Cnpj);
                if (existsCpf != null)
                    throw new HttpRequestException("O CPF novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            if (userRequest.Email != email)
            {
                var existsEmail = await _userRepository.GetByEmail(userRequest.Email, userRequest.Cnpj);
                if (existsEmail != null)
                    throw new HttpRequestException("O Email novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            userForUpdate.Cpf = userRequest.Cpf;
            userForUpdate.Cnpj = userRequest.Cnpj;
            userForUpdate.Rg = userRequest.Rg;
            userForUpdate.Name = userRequest.Name;
            userForUpdate.Email = userRequest.Email;
            userForUpdate.Role = userRequest.Role;
            userForUpdate.ProfileImageUrl = userRequest.ProfileImageUrl;
            userForUpdate.BornDate = userRequest.BornDate;
            userForUpdate.Cel = userRequest.Cel;
            userForUpdate.CivilStatus = userRequest.CivilStatus;
            userForUpdate.CpfDocumentUrl = userRequest.CpfDocumentUrl;
            userForUpdate.UpdatedAt = System.DateTime.UtcNow;
            userForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _userRepository.UpdateUser(userForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, string email, string cnpj)
        {
            var userForUpdate = await _userRepository.GetByEmail(email, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            userForUpdate.Status = status;
            userForUpdate.UpdatedAt = System.DateTime.UtcNow;
            userForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _userRepository.UpdateUser(userForUpdate) > 0) return true;
            else return false;
        }
    }
}