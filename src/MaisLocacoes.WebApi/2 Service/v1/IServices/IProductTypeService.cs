using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductTypeService
    {
        Task<ProductTypeResponse> CreateProductType(ProductTypeRequest productTypeRequest);
        Task<bool> DeleteById(int id);
    }
}