using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Product;

namespace Service.v1.IServices
{
    public interface IProductService
    {
        Task<CreateProductResponse> CreateProduct(CreateProductRequest productRequest);
        Task<GetProductByIdResponse> GetProductById(int id);
        Task<GetProductByTypeCodeResponse> GetProductByTypeCode(int typeId, string code);
        Task<IEnumerable<GetProductsByPageResponse>> GetProductsByPage(int items, int page, string query);
        Task<IEnumerable<GetProductsForRentResponse>> GetProductsForRent(int productTypeId);
        Task UpdateProduct(UpdateProductRequest productRequest, int id);
        Task UpdateStatus(string status, int id);
        Task DeleteById(int id);
    }
}