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

        public async Task<ProductEntity> GetByTypeCode(string type, string code) => await _context.Products.FirstOrDefaultAsync(p => p.ProductType.ToLower() == type.ToLower() && p.Code.ToLower() == code.ToLower());
        
        public async Task<bool> ProductExists(string type, string code) => await _context.Products.AnyAsync(p => p.ProductType.ToLower() == type.ToLower() && p.Code.ToLower() == code.ToLower());

        public async Task<int> UpdateProduct(ProductEntity productForUpdate)
        {
            _context.Products.Update(productForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}