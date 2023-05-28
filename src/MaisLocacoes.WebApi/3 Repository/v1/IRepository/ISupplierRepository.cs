using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface ISupplierRepository
    {
        Task<SupplierEntity> CreateSupplier(SupplierEntity supplierEntity);
        Task<SupplierEntity> GetById(int id);
        Task<int> UpdateSupplier(SupplierEntity supplierForUpdate);
    }
}