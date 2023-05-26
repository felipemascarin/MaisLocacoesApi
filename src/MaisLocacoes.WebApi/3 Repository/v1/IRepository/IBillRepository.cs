using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository
{
    public interface IBillRepository
    {
        Task<BillEntity> CreateBill(BillEntity billEntity);
    }
}