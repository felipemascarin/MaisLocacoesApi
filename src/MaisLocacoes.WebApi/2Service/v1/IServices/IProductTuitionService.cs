using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<ProductTuitionResponse> CreateProductTuition(ProductTuitionRequest productTuitionRequest);
        Task<GetProductTuitionRentResponse> GetById(int id);
        Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllByRentId(int rentId);
        Task<IEnumerable<GetProductTuitionRentResponse>> GetAllByProductId(int productId);
        Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllToRemove();
        Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id);
        Task<bool> UpdateProductCode(string productCode, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}