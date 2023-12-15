using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
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
            await context.Set<ProductTypeEntity>().AddAsync(productTypeEntity);
            context.SaveChanges();
            return productTypeEntity;
        }

        public async Task<ProductTypeEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<bool> ProductTypeExists(string type)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().AnyAsync(p => p.Type.ToLower() == type.ToLower() && p.Deleted == false);
        }

        public async Task<bool> ProductTypeExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().AnyAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<IEnumerable<ProductTypeEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().Where(p => p.Deleted == false).ToListAsync();
        }

        public async Task<int> UpdateProductType(ProductTypeEntity productTypeForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<ProductTypeEntity>().Update(productTypeForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}