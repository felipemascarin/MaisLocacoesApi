using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductTuitionRepository
    {
        Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity);
        Task<ProductTuitionEntity> GetById(int id);
        Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate);
    }
}