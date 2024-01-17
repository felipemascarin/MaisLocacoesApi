using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository.UserSchema
{
    public interface IUserRepository
    {
        Task<UserEntity> CreateUser(UserEntity userEntity);
        Task<bool> UserHasToken(string token, string email);
        Task<UserEntity> GetByEmail(string email);
        Task<UserEntity> GetByCpf(string cpf);
        Task<IEnumerable<UserEntity>> GetAllByEmailList(IEnumerable<string> email);
        Task<bool> UserExists(string email, string cpf);
        Task<int> UpdateUser(UserEntity userEntity);
    }
}