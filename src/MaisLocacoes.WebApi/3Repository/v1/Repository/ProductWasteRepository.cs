using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
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
        }

        public async Task<ProductWasteEntity> CreateProductWaste(ProductWasteEntity productWasteEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productWasteEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return productWasteEntity;
        }

        public async Task<ProductWasteEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductWasteEntity>().FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<IEnumerable<ProductWasteEntity>> GetAllByProductId(int productId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductWasteEntity>().Where(p => p.ProductId == productId && p.Deleted == false).OrderByDescending(p => p.Date).ToListAsync();
        }

        public async Task<IEnumerable<ProductWasteEntity>> GetProductWastesByPage(int items, int page, string query)
        {
            using var context = _contextFactory.CreateContext();
            if (query == null)
                return await context.Set<ProductWasteEntity>().Where(p => p.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await context.Set<ProductWasteEntity>().Where(p => p.Deleted == false && (
                     p.Product.ProductType.Type.ToLower().Contains(query.ToLower()) ||
                     p.Product.Code.ToLower().Contains(query.ToLower()) ||
                     p.Description.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<int> UpdateProductWaste(ProductWasteEntity productWasteForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productWasteForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}