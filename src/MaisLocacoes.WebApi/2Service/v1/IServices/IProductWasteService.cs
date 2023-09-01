using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IProductWasteService
    {
        Task<ProductWasteResponse> CreateProductWaste(ProductWasteRequest productWasteRequest);
        Task<ProductWasteResponse> GetById(int id);
        Task<GetProductWasteProductIdResponse> GetAllByProductId(int productId);
        Task<IEnumerable<ProductWasteResponse>> GetProductWastesByPage(int items, int page, string query);
        Task<bool> UpdateProductWaste(ProductWasteRequest productWasteRequest, int id);
        Task<bool> DeleteById(int id);
    }
}