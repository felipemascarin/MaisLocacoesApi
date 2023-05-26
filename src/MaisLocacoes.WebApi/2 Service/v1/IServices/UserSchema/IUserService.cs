using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;
using Repository.v1.Entity.UserSchema;
using System.Threading.Tasks;

namespace Service.v1.IServices.UserSchema
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(UserRequest userRequest);
        Task<GetUserResponse> GetByEmail(string email);
        Task<GetUserResponse> GetByCpf(string cpf);
        Task<bool> UpdateUser(UserRequest userRequest, string email);
        Task<bool> UpdateStatus(string status, string email);
        Task<bool> DeleteByEmail(string email);
    }
}