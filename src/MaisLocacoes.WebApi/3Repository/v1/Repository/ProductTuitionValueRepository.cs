using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace MaisLocacoes.WebApi._3_Repository.v1.Repository
{
    public class ProductTuitionValueRepository : IProductTuitionValueRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public ProductTuitionValueRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<ProductTuitionValueEntity> CreateProductTuitionValue(ProductTuitionValueEntity productTuitionValueEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTuitionValueEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return productTuitionValueEntity;
        }

        public async Task<ProductTuitionValueEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionValueEntity>().Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ProductTuitionValueExists(int productTypeId, int quantityPeriod, string timePeriod)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionValueEntity>().AnyAsync(p => p.ProductTypeId == productTypeId && p.QuantityPeriod == quantityPeriod && p.TimePeriod == timePeriod);
        }

        public async Task<IEnumerable<ProductTuitionValueEntity>> GetAllByProductTypeId(int productTypeId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionValueEntity>().Where(p => p.ProductTypeId == productTypeId).OrderBy(p => p.TimePeriod).ToListAsync();
        }

        public async Task<int> UpdateProductTuitionValue(ProductTuitionValueEntity productTuitionValueForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<ProductTuitionValueEntity>().Update(productTuitionValueForUpdate);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductTuitionValue(ProductTuitionValueEntity productTuitionValueForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTuitionValueForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}