using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductWasteRepository
    {
        Task<ProductWasteEntity> CreateProductWaste(ProductWasteEntity productWasteEntity);
        Task<ProductWasteEntity> GetById(int id);
        Task<int> UpdateProductWaste(ProductWasteEntity productWasteForUpdate);
    }
}