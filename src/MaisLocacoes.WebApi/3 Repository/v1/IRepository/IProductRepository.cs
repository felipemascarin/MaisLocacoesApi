using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductRepository
    {
        Task<ProductEntity> CreateProduct(ProductEntity productEntity);
        Task<ProductEntity> GetById(int id);
        Task<ProductEntity> GetByTypeCode(int typeId, string code);
        Task<bool> ProductExists(int typeId, string code);
        Task<int> UpdateProduct(ProductEntity productForUpdate);
    }
}