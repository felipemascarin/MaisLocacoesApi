using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductTuitionValueRepository
    {
        Task<ProductTuitionValueEntity> CreateProductTuitionValue(ProductTuitionValueEntity productTuitionValueEntity);
        Task<ProductTuitionValueEntity> GetById(int id);
        Task<bool> ProductTuitionValueExists(int productTypeId, int quantityPeriod, string timePeriod);
        Task<IEnumerable<ProductTuitionValueEntity>> GetAllByProductTypeId(int productTypeId);
        Task<int> UpdateProductTuitionValue(ProductTuitionValueEntity productTuitionValueForUpdate);
        Task<int> DeleteProductTuitionValue(ProductTuitionValueEntity productTuitionValueForDelete);
    }
}