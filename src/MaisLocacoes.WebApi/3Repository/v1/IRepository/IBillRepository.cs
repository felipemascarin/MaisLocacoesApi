using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository
{
    public interface IBillRepository
    {
        Task<BillEntity> CreateBill(BillEntity billEntity);
        Task<BillEntity> GetById(int id);
        Task<IEnumerable<BillEntity>> GetByRentId(int rentId);
        Task<int> UpdateBill(BillEntity billForUpdate);
    }
}