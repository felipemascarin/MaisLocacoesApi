using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication;

namespace MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        LoginResponse LoginAdm(LoginRequest loginRequest);
        Task<bool> Logout(LogoutRequest tokenRequest);
    }
}