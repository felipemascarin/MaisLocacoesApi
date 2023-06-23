using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductTuitionValueService
    {
        Task<ProductTuitionValueResponse> CreateProductTuitionValue(ProductTuitionValueRequest productTuitionValueRequest);
        Task<ProductTuitionValueResponse> GetById(int id);
        Task<IEnumerable<ProductTuitionValueResponse>> GetAllByProductTypeId(int productTypeId);
        Task<bool> UpdateProductTuitionValue(ProductTuitionValueRequest productTuitionValueRequest, int id);
        Task<bool> DeleteById(int id);
    }
}