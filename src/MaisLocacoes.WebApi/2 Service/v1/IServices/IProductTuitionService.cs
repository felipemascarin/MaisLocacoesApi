using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<ProductTuitionResponse> CreateProductTuition(ProductTuitionRequest productTuitionRequest);
        Task<ProductTuitionResponse> GetById(int id);
        Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id);
        Task<bool> DeleteById(int id);
    }
}