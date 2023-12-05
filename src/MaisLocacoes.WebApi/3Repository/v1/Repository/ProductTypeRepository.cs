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
        }

        public async Task<ProductTypeEntity> CreateProductType(ProductTypeEntity productTypeEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.ProductTypes.AddAsync(productTypeEntity);
            context.SaveChanges();
            return productTypeEntity;
        }

        public async Task<ProductTypeEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.ProductTypes.FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<bool> ProductTypeExists(string type)
        {
            using var context = _contextFactory.CreateContext();
            return await context.ProductTypes.AnyAsync(p => p.Type.ToLower() == type.ToLower() && p.Deleted == false);
        }

        public async Task<bool> ProductTypeExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.ProductTypes.AnyAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<IEnumerable<ProductTypeEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.ProductTypes.Where(p => p.Deleted == false).ToListAsync();
        }

        public async Task<int> UpdateProductType(ProductTypeEntity productTypeForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ProductTypes.Update(productTypeForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}