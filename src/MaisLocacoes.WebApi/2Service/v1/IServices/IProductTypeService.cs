using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;

namespace Service.v1.IServices
{
    public interface IProductTypeService
    {
        Task<CreateProductTypeResponse> CreateProductType(CreateProductTypeRequest productTypeRequest);
        Task<GetProductTypeByIdResponse> GetProductTypeById(int id);
        Task<IEnumerable<GetAllProductTypesResponse>> GetAllProductTypes();
        Task UpdateProductType(UpdateProductTypeRequest productTypeRequest, int id);
        Task DeleteById(int id);
    }
}