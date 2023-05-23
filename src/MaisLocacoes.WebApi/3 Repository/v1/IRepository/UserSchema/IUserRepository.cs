using Repository.v1.Entity.UserSchema;
using System.Threading.Tasks;

namespace Repository.v1.IRepository.UserSchema
{
    public interface IUserRepository
    {
        Task<UserEntity> CreateUser(UserEntity userEntity);
        Task<UserEntity> GetByEmail(string email);
        Task<bool> UserExists(string email, string cpf);
        Task<int> UpdateUser(UserEntity userEntity);
        Task<bool> UserHasToken(string token, string email);
    }
}