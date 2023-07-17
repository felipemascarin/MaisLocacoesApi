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

        public async Task<CreateTokenResponse> Login(LoginRequest loginRequest)
        {
            //Log entrou no método
            var email = JwtManager.ExtractEmailByToken(loginRequest.GoogleToken);

            var userEntity = await _userRepository.GetByEmail(email, loginRequest.Cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            var companyEntity = await _companyRepository.GetByCnpj(loginRequest.Cnpj) ??
               throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            if (userEntity.Status == UserStatus.UserStatusEnum.ElementAt(1))
            {
                throw new HttpRequestException("Usuário bloqueado", null, HttpStatusCode.Forbidden);
            }

            if (companyEntity.Status != CompanyStatus.CompanyStatusEnum.ElementAt(0))
                throw new HttpRequestException("Empresa sem acesso - Entrar em contato com suporte", null, HttpStatusCode.Forbidden);

            var user = new User()
            {
                Name = userEntity.Name,
                Cpf = userEntity.Cpf,
                Email = userEntity.Email,
                Role = userEntity.Role,
                Schema = userEntity.Cnpj,
                Module = companyEntity.Module
            };

            var tokenResponse = JwtManager.CreateToken(user);

            userEntity.LastToken = tokenResponse.Token;
            await _userRepository.UpdateUser(userEntity);

            return tokenResponse;
        }

        //Adicionar validações para entrar no end, se n estiver logado nem faz logout
        public async Task<bool> Logout(TokenRequest tokenRequest)
        {
            //Log entrou no método
            var email = JwtManager.ExtractEmailByToken(tokenRequest.Token);

            var cnpj = JwtManager.ExtractSchemaByToken(tokenRequest.Token);

            var userForUpdate = await _userRepository.GetByEmail(email, cnpj) ??
                throw new HttpRequestException("Usuário não encontrado", null, HttpStatusCode.NotFound);

            userForUpdate.LastToken = string.Empty;

            if (await _userRepository.UpdateUser(userForUpdate) > 0) return true;
            else return false;
        }
    }
}