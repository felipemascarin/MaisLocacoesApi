using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IRentRepository
    {
        Task<RentEntity> CreateRent(RentEntity rentEntity);
        Task<bool> RentExists(int id);
        Task<RentEntity> GetById(int id);
    }
}