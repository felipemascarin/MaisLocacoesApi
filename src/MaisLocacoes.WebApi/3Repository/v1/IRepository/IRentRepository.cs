using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IRentRepository
    {
        Task<RentEntity> CreateRent(RentEntity rentEntity);
        Task<RentEntity> GetById(int id);
        Task<bool> RentExists(int id);
        Task<IEnumerable<RentEntity>> GetAllByClientId(int clientId);
        Task<IEnumerable<RentEntity>> GetRentsByPage(int items, int page, string query, string status);
        Task<int> UpdateRent(RentEntity rentForUpdate);
    }
}