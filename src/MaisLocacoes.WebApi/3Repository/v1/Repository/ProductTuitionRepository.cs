using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ProductTuitionRepository : IProductTuitionRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionRepository(PostgreSqlContextFactory contextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory; 
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTuitionEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return productTuitionEntity;
        }

        public async Task<ProductTuitionEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>().Include(p => p.Rent).Include(p => p.Rent.Address).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<int> GetProductTuitionsQuantity(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
            .Where(r => r.RentId == rentId && r.Deleted == false).CountAsync();
        }

        public async Task<bool> ProductTuitionExists(int? id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>().AnyAsync(p => p.Id == id && p.Deleted == false);
        }

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByRentId(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
            .Include(p => p.ProductType)
            .Include(p => p.Product)
            .Include(p => p.Rent)
            .ThenInclude(p => p.Address)
            .Include(p => p.Rent.Client)
            .ThenInclude(p => p.Address)
            .Where(p => p.RentId == rentId && p.Deleted == false).OrderBy(p => p.FinalDateTime).ToListAsync();
        }

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByProductTypeCode(int productTypeId, string productCode)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>().Include(p => p.Rent).Include(p => p.Rent.Address).Where(p => p.ProductTypeId == productTypeId && p.ProductCode == productCode && p.Deleted == false).OrderBy(p => p.InitialDateTime).ToListAsync();
        }

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByProductIdForRentedPlaces(List<int> productIds)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
                .Where(p => productIds.Contains(p.ProductId.Value) && (p.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2) /*delivered*/ || p.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4)) /*withdraw*/ && p.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllToRemove(DateTime todayDate)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
            .Include(p => p.ProductType)
            .Include(p => p.Rent)
            .ThenInclude(p => p.Address)
            .Include(p => p.Rent.Client)
            .ThenInclude(p => p.Address)
            .Where(p => p.FinalDateTime.Date <= todayDate && p.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2) /*delivered*/ && p.Deleted == false).ToListAsync();
        }

        public async Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
                .AnyAsync(p => p.RentId == rentId && p.ProductTypeId == productTypeId &&
                p.ProductCode.ToLower() == productCode.ToLower() &&
                p.Status != ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5) /*returned*/ &&
                p.Deleted == false);
        }

        public async Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(productTuitionForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}