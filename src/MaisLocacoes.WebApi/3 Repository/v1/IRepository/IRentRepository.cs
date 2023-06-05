using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository
{
    public interface IRentRepository
    {
        Task<RentEntity> CreateRent(RentEntity rentEntity);
        Task<RentEntity> GetById(int id);
        Task<bool> RentExists(int id);
        Task<IEnumerable<RentEntity>> GetAllByClientId(int clientId);
        Task<IEnumerable<RentEntity>> GetRentsByPage(int items, int page, string query);
        Task<int> UpdateRent(RentEntity rentForUpdate);
    }
}