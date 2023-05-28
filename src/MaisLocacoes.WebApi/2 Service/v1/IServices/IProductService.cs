using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest productRequest);
        Task<ProductResponse> GetById(int id);
        Task<ProductResponse> GetByTypeCode(int typeId, string code);
        Task<bool> UpdateProduct(ProductRequest productRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}