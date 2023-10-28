using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IProductWasteService
    {
        Task<CreateProductWasteResponse> CreateProductWaste(CreateProductWasteRequest productWasteRequest);
        Task<GetProductWasteByIdResponse> GetProductWasteById(int id);
        Task<GetAllProductWastesByProductIdResponse> GetAllProductWastesByProductId(int productId);
        Task<IEnumerable<GetProductWastesByPageResponse>> GetProductWastesByPage(int items, int page, string query);
        Task UpdateProductWaste(UpdateProductWasteRequest productWasteRequest, int id);
        Task DeleteById(int id);
    }
}