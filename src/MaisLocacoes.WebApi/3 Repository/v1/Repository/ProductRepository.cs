using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostgreSqlContext _context;

        public ProductRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> CreateProduct(ProductEntity productEntity)
        {
            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();
            return productEntity;
        }

        public async Task<ProductEntity> GetById(int id) => await _context.Products.Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<ProductEntity> GetByTypeCode(int typeId, string code) => await _context.Products.Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower() && p.Deleted == false);
        
        public async Task<bool> ProductExists(int typeId, string code) => await _context.Products.AnyAsync(p => p.ProductTypeId == typeId && p.Code.ToLower() == code.ToLower() && p.Deleted == false);
        
        public async Task<bool> ProductExists(int id) => await _context.Products.AnyAsync(p => p.ProductTypeId == id && p.Deleted == false);

        public async Task<IEnumerable<ProductEntity>> GetProductsByPage(int items, int page, string query)
        {
            if (query == null)
                return await _context.Products.Include(p => p.ProductTypeEntity).Where(p => p.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await _context.Products.Include(p => p.ProductTypeEntity).Where(p => p.Deleted == false && (
                     p.Code.ToLower().Contains(query.ToLower()) ||
                     p.Description.ToLower().Contains(query.ToLower()) ||
                     p.ProductTypeEntity.Type.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<int> UpdateProduct(ProductEntity productForUpdate)
        {
            _context.Products.Update(productForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}