using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest productRequest);
        Task<ProductResponse> GetByTypeCode(string type, string code);
        Task<bool> UpdateProduct(ProductRequest productRequest, string type, string code);
    }
}