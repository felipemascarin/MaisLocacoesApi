using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuitionValue;

namespace Service.v1.IServices
{
    public interface IProductTuitionValueService
    {
        Task<CreateProductTuitionValueResponse> CreateProductTuitionValue(CreateProductTuitionValueRequest productTuitionValueRequest);
        Task<GetProductTuitionValueByIdResponse> GetProductTuitionValueById(int id);
        Task<IEnumerable<GetAllProductTuitionValueByProductTypeIdResponse>> GetAllProductTuitionValueByProductTypeId(int productTypeId);
        Task UpdateProductTuitionValue(UpdateProductTuitionValueRequest productTuitionValueRequest, int id);
        Task DeleteById(int id);
    }
}