using AutoMapper;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity.UserSchema;
using MaisLocacoes.WebApi._3_Repository.v1.IRepository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using Repository.v1.Repository.UserSchema;
using Service.v1.IServices.UserSchema;
using System.Net;

namespace Service.v1.Services.UserSchema
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDeletionsRepository _deletionsRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICompanyRepository companyRepository,
            IDeletionsRepository deletionsRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
            _deletionsRepository = deletionsRepository;
        }

        public async Task<CreateUserResponse> CreateUser(UserRequest userRequest)
        {
            var existsUser = await _userRepository.UserExists(userRequest.Email, userRequest.Cpf);
            if (existsUser)
                throw new HttpRequestException("Cpf ou e-mail de usuário já cadastrado", null, HttpStatusCode.BadRequest);

            var existsCompany = await _companyRepository.CompanyExists(userRequest.Cnpj);
            if (!existsCompany)
                throw new HttpRequestException("Não existe nenhuma empresa cadastrada com esse CNPJ", null, HttpStatusCode.BadRequest);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            userEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            userEntity = await _userRepository.CreateUser(userEntity);

            return _mapper.Map<CreateUserResponse>(userEntity);
        }

        public async Task<GetUserResponse> GetByEmail(string email)
        {
            var userEntity = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<GetUserResponse>(userEntity);

            return userResponse;
        }

        public async Task<GetUserResponse> GetByCpf(string cpf)
        {
            var userEntity = await _userRepository.GetByCpf(cpf) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<GetUserResponse>(userEntity);

            return userResponse;
        }

        public async Task<bool> UpdateUser(UserRequest userRequest, string email)
        {
            var userForUpdate = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            if (userRequest.Cpf != userForUpdate.Cpf)
            {
                var existsCpf = await _userRepository.GetByCpf(userRequest.Cpf);
                if (existsCpf != null)
                    throw new HttpRequestException("O Cnpj novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            if (userRequest.Email != email)
            {
                var existsEmail = await _userRepository.GetByEmail(userRequest.Email);
                if (existsEmail != null)
                    throw new HttpRequestException("O Email novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            if (userRequest.Cnpj != userForUpdate.Cnpj)
            {
                var existsCompany = await _companyRepository.CompanyExists(userRequest.Cnpj);
                if (!existsCompany)
                    throw new HttpRequestException("Não existe nenhuma empresa cadastrada com esse CNPJ", null, HttpStatusCode.BadRequest);
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

        public async Task<bool> UpdateStatus(string status, string email)
        {
            var userForUpdate = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            userForUpdate.Status = status;
            userForUpdate.UpdatedAt = System.DateTime.UtcNow;
            userForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _userRepository.UpdateUser(userForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteByEmail(string email)
        {
            var userEntity = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userForDelete = _mapper.Map<UsersDeletions>(userEntity);
            userForDelete.DeletedAt = System.DateTime.UtcNow;
            userForDelete.DeletedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            await _deletionsRepository.CreateUsersDeletions(userForDelete);

            if (await _deletionsRepository.DeleteUser(userEntity) > 0) return true;
            else return false;
        }
    }
}