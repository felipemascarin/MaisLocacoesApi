using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserRequest userRequest);
        Task<UserResponse> GetByEmail(string email, string cnpj);
        Task<UserResponse> GetByCpf(string cpf, string cnpj);
        Task<IEnumerable<UserResponse>> GetAllByCnpj(string cnpj);
        Task<bool> UpdateUser(UserRequest userRequest, string email, string cnpj);
        Task<bool> UpdateStatus(string status, string email, string cnpj);
    }
}