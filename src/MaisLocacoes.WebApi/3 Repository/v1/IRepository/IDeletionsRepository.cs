using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity.UserSchema;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi._3_Repository.v1.IRepository
{
    public interface IDeletionsRepository
    {
        Task<ClientsDeletions> CreateClientsDeletions(ClientsDeletions clientForDelete);
        Task<int> DeleteClient(ClientEntity clientForDelete);
        Task<UsersDeletions> CreateUsersDeletions(UsersDeletions userForDelete);
        Task<int> DeleteUser(UserEntity userForDelete);
    }
}
