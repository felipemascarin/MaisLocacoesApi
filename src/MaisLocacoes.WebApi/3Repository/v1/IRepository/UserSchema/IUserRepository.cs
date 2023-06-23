using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using Repository.v1.Entity.UserSchema;
using System.Threading.Tasks;

namespace Repository.v1.IRepository.UserSchema
{
    public interface IUserRepository
    {
        Task<UserEntity> CreateUser(UserEntity userEntity);
        Task<bool> UserHasToken(string token, string email, string cnpj);
        Task<UserEntity> GetByEmail(string email, string cnpj);
        Task<UserEntity> GetByCpf(string cpf, string cnpj);
        Task<bool> UserExists(string email, string cpf, string cnpj);
        Task<int> UpdateUser(UserEntity userEntity);
    }
}