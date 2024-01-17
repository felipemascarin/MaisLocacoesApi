using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
using MaisLocacoes.WebApi._3Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository.UserSchema;
using System.Net;

namespace MaisLocacoes.WebApi._2_Service.v1.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private const double TOKENHOURS = 15;
        private const string ADMEMAIL = "luisfelipemascarinalmeida@gmail.com";
        private const string ADMNAME = "adm";
        private const string ADMCPF = "adm";
        private const string ADMTIMEZONE = "America/Sao_Paulo";
        private const string ADMCNPJ = "maislocacoes";
        private const string ADMDATABASE = "maislocacoes";

        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyUserRepository _companyUserRepository;

        public AuthenticationService(IUserRepository userRepository, ICompanyRepository companyRepository, ICompanyUserRepository companyUserRepository)
        {
            _userRepository = userRepository;
            _companyUserRepository = companyUserRepository;
            _companyRepository = companyRepository;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var userEmail = JwtManager.ExtractPropertyByToken(loginRequest.GoogleToken, "email");

            if (userEmail == ADMEMAIL)
            {
                var admUser = new User()
                {
                    Name = ADMNAME,
                    Cpf = ADMCPF,
                    Email = userEmail,
                    Role = UserRole.PersonRolesEnum.ElementAt(3), //role adm
                    Cnpj = ADMCNPJ,
                    Module = ProjectModules.Modules.ElementAt(2),
                    TimeZone = ADMTIMEZONE,
                    DataBase = ADMDATABASE
                };

                var loginResponse = JwtManager.CreateToken(admUser, 1);
                loginResponse.Cnpjs = new List<string>() { admUser.Cnpj };

                return loginResponse;
            }

            var userEntity = await _userRepository.GetByEmail(userEmail) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            if (userEntity.Status == UserStatus.UserStatusEnum.ElementAt(1) /*blocked*/)
                throw new HttpRequestException("Usuário bloqueado", null, HttpStatusCode.Forbidden);

            var userCnpjList = await _companyUserRepository.GetCnpjListByEmail(userEmail);
            if (!userCnpjList.Any())
                throw new HttpRequestException("Nenhuma empresa encontrada para esse usuário", null, HttpStatusCode.NotFound);

            if (userCnpjList.Count() == 1)
            {
                var companyEntity = await _companyRepository.GetByCnpj(userCnpjList.ElementAt(0)) ??
                    throw new HttpRequestException("Empresa não encontrada no banco de dados para o cnpj cadastrado nesse usuário", null, HttpStatusCode.NotFound);

                //Se o status da Empresa for diferente de regular, não deixa loggar
                if (companyEntity.Status != CompanyStatus.CompanyStatusEnum.ElementAt(0) /*regular*/)
                    throw new HttpRequestException("Empresa sem acesso - Entrar em contato com suporte", null, HttpStatusCode.Forbidden);

                var user = new User()
                {
                    Name = userEntity.Name,
                    Cpf = userEntity.Cpf,
                    Email = userEntity.Email,
                    Role = userEntity.Role,
                    Cnpj = companyEntity.Cnpj,
                    Module = companyEntity.Module,
                    TimeZone = companyEntity.TimeZone.ToString(),
                    DataBase = companyEntity.DataBase
                };

                var loginResponse = JwtManager.CreateToken(user, TOKENHOURS);
                loginResponse.Cnpjs = userCnpjList.ToList();

                //Salva o novo token gerado na propriedade LastToken da tabela Users no banco de dados
                userEntity.LastToken = loginResponse.Token;
                await _userRepository.UpdateUser(userEntity);

                return loginResponse;
            }
            else
            {
                var user = new User()
                {
                    Name = userEntity.Name,
                    Cpf = userEntity.Cpf,
                    Email = userEntity.Email,
                    Role = userEntity.Role,
                    Cnpj = "",
                    Module = "",
                    TimeZone = "",
                    DataBase = ""
                };

                var loginResponse = JwtManager.CreateToken(user, TOKENHOURS);
                loginResponse.Cnpjs = userCnpjList.ToList();

                return loginResponse;
            }
        }

        public async Task<LoginEnviromentResponse> LoginEnviroment(LoginEnviromentRequest modifyLoginTokenRequest)
        {
            var userEmail = JwtManager.ExtractPropertyByToken(modifyLoginTokenRequest.Token, "email");

            var userEntity = await _userRepository.GetByEmail(userEmail) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            if (userEntity.Status == UserStatus.UserStatusEnum.ElementAt(1) /*blocked*/)
                throw new HttpRequestException("Usuário bloqueado", null, HttpStatusCode.Forbidden);

            var companyEntity = await _companyRepository.GetByCnpj(modifyLoginTokenRequest.Cnpj) ??
                throw new HttpRequestException("Empresa não encontrada no banco de dados para o cnpj cadastrado nesse usuário", null, HttpStatusCode.NotFound);

            if (companyEntity.Status != CompanyStatus.CompanyStatusEnum.ElementAt(0) /*regular*/)
                throw new HttpRequestException("Empresa sem acesso - Entrar em contato com suporte", null, HttpStatusCode.Forbidden);

            var user = new User()
            {
                Name = userEntity.Name,
                Cpf = userEntity.Cpf,
                Email = userEntity.Email,
                Role = userEntity.Role,
                Cnpj = companyEntity.Cnpj,
                Module = companyEntity.Module,
                TimeZone = companyEntity.TimeZone.ToString(),
                DataBase = companyEntity.DataBase
            };

            var newToken = JwtManager.CreateToken(user, TOKENHOURS);

            var modifyLoginTokenResponse = new LoginEnviromentResponse()
            {
                Token = newToken.Token
            };

            //Salva o novo token gerado na propriedade LastToken da tabela Users no banco de dados
            userEntity.LastToken = modifyLoginTokenResponse.Token;
            await _userRepository.UpdateUser(userEntity);

            return modifyLoginTokenResponse;
        }

        public async Task Logout(LogoutRequest tokenRequest)
        {
            var email = JwtManager.ExtractPropertyByToken(tokenRequest.Token, "email");

            var userForUpdate = await _userRepository.GetByEmail(email) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            //Limpa a propriedade LastToken da tabela Users no banco de dados
            userForUpdate.LastToken = string.Empty;

            await _userRepository.UpdateUser(userForUpdate);
        }
    }
}