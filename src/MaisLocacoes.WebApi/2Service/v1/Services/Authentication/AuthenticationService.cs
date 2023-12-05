using MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication;
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
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;

        public AuthenticationService(IUserRepository userRepository, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _companyRepository = companyRepository;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var email = JwtManager.ExtractPropertyByToken(loginRequest.GoogleToken, "email");

            var userEntity = await _userRepository.GetByEmail(email, loginRequest.Cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var companyEntity = await _companyRepository.GetByCnpj(loginRequest.Cnpj) ??
               throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            //Se o status do usuário for Blocked, não permite logar
            if (userEntity.Status == UserStatus.UserStatusEnum.ElementAt(1))
            {
                throw new HttpRequestException("Usuário bloqueado", null, HttpStatusCode.Forbidden);
            }

            //Se o status da Empresa for diferente de regular, não deixa loggar
            if (companyEntity.Status != CompanyStatus.CompanyStatusEnum.ElementAt(0))
                throw new HttpRequestException("Empresa sem acesso - Entrar em contato com suporte", null, HttpStatusCode.Forbidden);

            var user = new User()
            {
                Name = userEntity.Name,
                Cpf = userEntity.Cpf,
                Email = userEntity.Email,
                Role = userEntity.Role,
                Cnpj = userEntity.Cnpj,
                Module = companyEntity.Module,
                TimeZone = companyEntity.TimeZone.ToString()
            };

            var tokenResponse = JwtManager.CreateToken(user, 15);

            //Salva o novo token gerado na propriedade LastToken da tabela Users no banco de dados
            userEntity.LastToken = tokenResponse.Token;
            await _userRepository.UpdateUser(userEntity);

            return tokenResponse;
        }

        public LoginResponse LoginAdm(LoginRequest loginRequest)
        {
            var email = JwtManager.ExtractPropertyByToken(loginRequest.GoogleToken, "email");

            var user = new User()
            {
                Name = "adm",
                Cpf = "adm",
                Email = email,
                Role = UserRole.PersonRolesEnum.ElementAt(3), //role adm
                Cnpj = "maislocacoes",
                Module = ProjectModules.Modules.ElementAt(2),
                TimeZone = "America/Sao_Paulo"
            };

            var tokenResponse = JwtManager.CreateToken(user, 1);

            return tokenResponse;
        }

        public async Task Logout(LogoutRequest tokenRequest)
        {
            var email = JwtManager.ExtractPropertyByToken(tokenRequest.Token, "email");

            var cnpj = JwtManager.ExtractPropertyByToken(tokenRequest.Token, "cnpj");

            var userForUpdate = await _userRepository.GetByEmail(email, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            //Limpa a propriedade LastToken da tabela Users no banco de dados
            userForUpdate.LastToken = string.Empty;

            await _userRepository.UpdateUser(userForUpdate);
        }
    }
}