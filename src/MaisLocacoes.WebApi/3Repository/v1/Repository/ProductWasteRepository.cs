using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductWasteRepository : IProductWasteRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public ProductWasteRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
            using var _context = _contextFactory.CreateContext();
        }

        public async Task<ProductWasteEntity> CreateProductWaste(ProductWasteEntity productWasteEntity)
        {
            await _context.ProductWastes.AddAsync(productWasteEntity);
            _context.SaveChanges();
            return productWasteEntity;
        }

        public async Task<ProductWasteEntity> GetById(int id) => await _context.ProductWastes.FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<IEnumerable<ProductWasteEntity>> GetAllByProductId(int productId) => await _context.ProductWastes.Where(p => p.ProductId == productId && p.Deleted == false).OrderByDescending(p => p.Date).ToListAsync();

        public async Task<IEnumerable<ProductWasteEntity>> GetProductWastesByPage(int items, int page, string query)
        {
            if (query == null)
                return await _context.ProductWastes.Where(p => p.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await _context.ProductWastes.Where(p => p.Deleted == false && (
                     p.ProductEntity.ProductTypeEntity.Type.ToLower().Contains(query.ToLower()) ||
                     p.ProductEntity.Code.ToLower().Contains(query.ToLower()) ||
                     p.Description.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<int> UpdateProductWaste(ProductWasteEntity productWasteForUpdate)
        {
            _context.ProductWastes.Update(productWasteForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}