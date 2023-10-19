using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest userRequest);
        Task<GetUserByEmailResponse> GetUserByEmail(string email, string cnpj);
        Task<GetUserByCpfResponse> GetUserByCpf(string cpf, string cnpj);
        Task<IEnumerable<GetAllUsersByCnpjResponse>> GetAllUsersByCnpj(string cnpj);
        Task<bool> UpdateUser(UpdateUserRequest userRequest, string email, string cnpj);
        Task<bool> UpdateStatus(string status, string email, string cnpj);
    }
}