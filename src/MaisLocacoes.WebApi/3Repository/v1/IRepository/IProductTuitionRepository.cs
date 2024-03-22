using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductTuitionRepository
    {
        Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity);
        Task<ProductTuitionEntity> GetById(int id);
        Task<int> GetProductTuitionsQuantity(int rentId);
        Task<bool> ProductTuitionExists(int? id);
        Task<IEnumerable<ProductTuitionEntity>> GetAllByRentId(int rentId);
        Task<IEnumerable<ProductTuitionEntity>> GetAllByProductTypeCode(int productTypeId, string productCode);
        Task<IEnumerable<ProductTuitionEntity>> GetAllByProductIdForRentedPlaces(List<int> productIds);
        Task<IEnumerable<ProductTuitionEntity>> GetAllToRemove(DateTime todayDate);
        Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode);
        Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate);
        Task<int> DeleteProductTuition(ProductTuitionEntity productTuitionForDelete);
    }
}