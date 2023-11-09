using Repository.v1.Entity;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Product.GetProductsForRentResponse;

namespace Repository.v1.IRepository
{
    public interface IProductRepository
    {
        Task<ProductEntity> CreateProduct(ProductEntity productEntity);
        Task<ProductEntity> GetById(int id);
        Task<ProductEntity> GetByTypeCode(int typeId, string code);
        Task<bool> ProductExists(int typeId, string code);
        Task<bool> ProductExists(int id);
        Task<IEnumerable<ProductEntity>> GetProductsByPage(int items, int page, string query);
        Task<IEnumerable<GetProductForRentDtoResponse>> GetProductsForRent(int productTypeId);
        Task<IEnumerable<ProductEntity>> GetProductsByProductCodeList(List<string> productCodeList);
        Task<ProductEntity> GetTheLastsCreated(int productTypeId);
        Task<int> UpdateProduct(ProductEntity productForUpdate);
    }
}