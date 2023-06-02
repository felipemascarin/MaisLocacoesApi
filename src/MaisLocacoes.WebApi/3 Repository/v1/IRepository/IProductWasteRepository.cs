using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IProductWasteRepository
    {
        Task<ProductWasteEntity> CreateProductWaste(ProductWasteEntity productWasteEntity);
        Task<ProductWasteEntity> GetById(int id);
        Task<IEnumerable<ProductWasteEntity>> GetAllById(int id);
        Task<IEnumerable<ProductWasteEntity>> GetProductWastesByPage(int items, int page, string query);
        Task<int> UpdateProductWaste(ProductWasteEntity productWasteForUpdate);
    }
}