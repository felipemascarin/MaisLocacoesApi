using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public ProductTypeRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
            using var _context = _contextFactory.CreateContext();
        }

        public async Task<ProductTypeEntity> CreateProductType(ProductTypeEntity productTypeEntity)
        {
            await _context.ProductTypes.AddAsync(productTypeEntity);
            _context.SaveChanges();
            return productTypeEntity;
        }

        public async Task<ProductTypeEntity> GetById(int id) => await _context.ProductTypes.FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);

        public async Task<bool> ProductTypeExists(string type) => await _context.ProductTypes.AnyAsync(p => p.Type.ToLower() == type.ToLower() && p.Deleted == false);

        public async Task<bool> ProductTypeExists(int id) => await _context.ProductTypes.AnyAsync(p => p.Id == id && p.Deleted == false);

        public async Task<IEnumerable<ProductTypeEntity>> GetAll() => await _context.ProductTypes.Where(p => p.Deleted == false).ToListAsync();

        public async Task<int> UpdateProductType(ProductTypeEntity productTypeForUpdate)
        {
            _context.ProductTypes.Update(productTypeForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}