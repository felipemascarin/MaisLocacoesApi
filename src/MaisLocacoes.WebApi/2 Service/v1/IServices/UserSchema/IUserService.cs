using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserRequest userRequest);
        Task<UserResponse> GetByEmail(string email);
        Task<UserResponse> GetByCpf(string cpf);
        Task<bool> UpdateUser(UserRequest userRequest, string email);
        Task<bool> UpdateStatus(string status, string email);
    }
}