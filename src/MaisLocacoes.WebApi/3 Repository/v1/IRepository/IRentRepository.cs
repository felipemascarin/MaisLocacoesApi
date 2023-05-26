using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository
{
    public interface IRentRepository
    {
        Task<RentEntity> CreateRent(RentEntity rentEntity);
        Task<bool> RentExists(int id);
        Task<RentEntity> GetById(int id);
        Task<int> UpdateRent(RentEntity rentForUpdate);
    }
}