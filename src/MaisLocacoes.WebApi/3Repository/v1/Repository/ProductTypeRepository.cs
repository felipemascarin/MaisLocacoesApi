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
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTypeEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return productTypeEntity;
        }

        public async Task<ProductTypeEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>()
            .Include(p => p.Products)
            .Include(p => p.ProductTuitions)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ProductTypeExists(string type)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().AnyAsync(p => p.Type.ToLower() == type.ToLower());
        }

        public async Task<bool> ProductTypeExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ProductTypeEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTypeEntity>().ToListAsync();
        }

        public async Task<int> UpdateProductType(ProductTypeEntity productTypeForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTypeForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductType(ProductTypeEntity productTypeForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTypeForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}