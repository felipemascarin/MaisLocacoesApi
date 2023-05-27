using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductRepository
    {
        Task<ProductEntity> CreateProduct(ProductEntity productEntity);
        Task<ProductEntity> GetByTypeCode(string type, string code);
        Task<bool> ProductExists(string type, string code);
        Task<int> UpdateProduct(ProductEntity productForUpdate);
    }
}