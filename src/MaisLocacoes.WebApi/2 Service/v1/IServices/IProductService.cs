using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IProductService
    {
        Task<CreateProductResponse> CreateProduct(ProductRequest productRequest);
        Task<GetProductResponse> GetByTypeCode(string type, string code);
        Task<bool> UpdateProduct(ProductRequest productRequest, string type, string code);
    }
}