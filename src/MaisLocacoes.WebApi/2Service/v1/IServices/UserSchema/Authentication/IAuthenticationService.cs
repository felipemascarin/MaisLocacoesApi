using MaisLocacoes.WebApi.Domain.Models.v1.Request.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema.Authentication;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Authentication;
using System.Threading.Tasks;

namespace MaisLocacoes.WebApi._2_Service.v1.IServices.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<bool> Logout(LogoutRequest tokenRequest);
    }
}