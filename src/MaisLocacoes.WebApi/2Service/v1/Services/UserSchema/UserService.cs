using AutoMapper;
using MaisLocacoes.WebApi._3Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi._3Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.User;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using Service.v1.IServices.UserSchema;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services.UserSchema
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICompanyRepository companyRepository,
            ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest userRequest)
        {
            if (userRequest.Role != UserRole.PersonRolesEnum.ElementAt(0) /*owner*/ && userRequest.Cnpjs.Count > 1)
                throw new HttpRequestException("Somente contas do tipo owner podem ter mais de 1 cnpj", null, HttpStatusCode.BadRequest);

            var existsUser = await _userRepository.UserExists(userRequest.Email, userRequest.Cpf);
            if (existsUser)
                throw new HttpRequestException("Cpf ou e-mail de usuário já cadastrado", null, HttpStatusCode.BadRequest);

            var userEntity = _mapper.Map<UserEntity>(userRequest);

            userEntity.BornDate = userEntity.BornDate.Value.Date;
            userEntity.CreatedBy = _email;
            userEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            userEntity = await _userRepository.CreateUser(userEntity);

            foreach (var cnpj in userRequest.Cnpjs)
            {
                var CompanyUserEntity = new CompanyUserEntity() { Cnpj = cnpj, Email = userRequest.Email };
                await _companyUserRepository.CreateCompanyUser(CompanyUserEntity);
            }

            return _mapper.Map<CreateUserResponse>(userEntity);
        }

        public async Task<GetUserByEmailResponse> GetUserByEmail(string email)
        {
            var userEntity = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<GetUserByEmailResponse>(userEntity);

            userResponse.Cnpjs = (await _companyUserRepository.GetCnpjListByEmail(email)).ToList();

            return userResponse;
        }

        public async Task<GetUserByCpfResponse> GetUserByCpf(string cpf)
        {
            var userEntity = await _userRepository.GetByCpf(cpf) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var userResponse = _mapper.Map<GetUserByCpfResponse>(userEntity);

            userResponse.Cnpjs = (await _companyUserRepository.GetCnpjListByEmail(userEntity.Email)).ToList();

            return userResponse;
        }

        public async Task<IEnumerable<GetAllUsersByCnpjResponse>> GetAllUsersByCnpj(string cnpj)
        {
            var emails = await _companyUserRepository.GetEmailListByCnpj(cnpj);

            var userEntities = await _userRepository.GetAllByEmailList(emails);

            var usersResponse = _mapper.Map<IEnumerable<GetAllUsersByCnpjResponse>>(userEntities);

            foreach (var userEntity in userEntities)
            {
                foreach (var user in usersResponse)
                {
                    if (userEntity.Cpf == user.Cpf)
                        user.Cnpjs = new List<string>();

                    foreach (var companyCnpj in userEntity.CompaniesUsers)
                    {
                        if (userEntity.Cpf == user.Cpf)
                            user.Cnpjs.Add(companyCnpj.Cnpj);
                    }
                }
            }

            return usersResponse;
        }

        public async Task UpdateUser(UpdateUserRequest userRequest, string email)
        {
            var userForUpdate = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            if (userRequest.Role != UserRole.PersonRolesEnum.ElementAt(0) /*owner*/ && userRequest.Cnpjs.Count > 1)
                throw new HttpRequestException("Somente contas do tipo owner podem ter mais de 1 cnpj", null, HttpStatusCode.BadRequest);

            if (userRequest.Cpf != userForUpdate.Cpf)
            {
                var existsCpf = await _userRepository.GetByCpf(userRequest.Cpf);
                if (existsCpf != null)
                    throw new HttpRequestException("O CPF novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            if (userRequest.Email != email)
            {
                var existsEmail = await _userRepository.GetByEmail(userRequest.Email);
                if (existsEmail != null)
                    throw new HttpRequestException("O Email novo já está cadastrado em outro usuário", null, HttpStatusCode.BadRequest);
            }

            userForUpdate.Cpf = userRequest.Cpf;
            userForUpdate.Rg = userRequest.Rg;
            userForUpdate.Name = userRequest.Name;
            userForUpdate.Email = userRequest.Email;
            userForUpdate.Role = userRequest.Role;
            userForUpdate.ProfileImageUrl = userRequest.ProfileImageUrl;
            userForUpdate.BornDate = userRequest.BornDate.Value.Date;
            userForUpdate.Cel = userRequest.Cel;
            userForUpdate.CivilStatus = userRequest.CivilStatus;
            userForUpdate.CpfDocumentUrl = userRequest.CpfDocumentUrl;
            userForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            userForUpdate.UpdatedBy = _email;

            var companiesUsersEntityList = (await _companyUserRepository.GetCnpjListByEmail(email)).ToList();

            foreach (var cnpj in companiesUsersEntityList)
            {
                if (!userRequest.Cnpjs.Contains(cnpj))
                {
                    var companyUserForDelete = new CompanyUserEntity() { Cnpj = cnpj, Email = email };
                    await _companyUserRepository.DeleteCompanyUser(companyUserForDelete);
                }
            }

            foreach (var cnpjRequest in userRequest.Cnpjs)
            {
                if (!companiesUsersEntityList.Contains(cnpjRequest))
                {
                    var companyUser = new CompanyUserEntity() { Cnpj = cnpjRequest, Email = email };
                    await _companyUserRepository.CreateCompanyUser(companyUser);
                }
            }

            await _userRepository.UpdateUser(userForUpdate);
        }

        public async Task UpdateStatus(string status, string email)
        {
            var userForUpdate = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            userForUpdate.Status = status;
            userForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            userForUpdate.UpdatedBy = _email;

            await _userRepository.UpdateUser(userForUpdate);
        }
    }
}