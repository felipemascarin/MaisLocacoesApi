using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductTuitionRepository
    {
        Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity);
        Task<ProductTuitionEntity> GetById(int id);
        Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode);
        Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate);
    }
}