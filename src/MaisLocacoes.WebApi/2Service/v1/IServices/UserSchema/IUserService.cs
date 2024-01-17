using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.User;

namespace Service.v1.IServices.UserSchema
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest userRequest);
        Task<GetUserByEmailResponse> GetUserByEmail(string email);
        Task<GetUserByCpfResponse> GetUserByCpf(string cpf);
        Task<IEnumerable<GetAllUsersByCnpjResponse>> GetAllUsersByCnpj(string cnpj);
        Task UpdateUser(UpdateUserRequest userRequest, string email);
        Task UpdateStatus(string status, string email);
    }
}