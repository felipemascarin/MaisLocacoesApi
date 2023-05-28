using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductTypeRepository
    {
        Task<ProductTypeEntity> CreateProductType(ProductTypeEntity productTypeEntity);
        Task<ProductTypeEntity> GetById(int id);
        Task<bool> ProductTypeExists(string type);
        Task<bool> ProductTypeExists(int id);
        Task<int> UpdateProductType(ProductTypeEntity productTypeForUpdate);
    }
}