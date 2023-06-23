using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IProductTypeService
    {
        Task<ProductTypeResponse> CreateProductType(ProductTypeRequest productTypeRequest);
        Task<ProductTypeResponse> GetById(int id);
        Task<IEnumerable<ProductTypeResponse>> GetAll();
        Task<bool> UpdateProductType(ProductTypeRequest productTypeRequest, int id);
        Task<bool> DeleteById(int id);
    }
}