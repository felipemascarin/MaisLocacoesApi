using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using TimeZoneConverter;

namespace Repository.v1.Repository
{
    public class ProductTuitionRepository : IProductTuitionRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 
        private readonly TimeZoneInfo _timeZone;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionRepository(PostgreSqlContextFactory contextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory; 
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
        }

        public async Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Set<ProductTuitionEntity>().AddAsync(productTuitionEntity);
            context.SaveChanges();
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

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllToRemove()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>()
            .Include(p => p.ProductType)
            .Include(p => p.Rent)
            .ThenInclude(p => p.Address)
            .Include(p => p.Rent.Client)
            .ThenInclude(p => p.Address)
            .Where(p => p.FinalDateTime.Date <= (TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone)).Date && p.Deleted == false).ToListAsync();
        }

        public async Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ProductTuitionEntity>().AnyAsync(p => p.RentId == rentId && p.ProductTypeId == productTypeId && p.ProductCode.ToLower() == productCode.ToLower() && p.Deleted == false);
        }

        public async Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<ProductTuitionEntity>().Update(productTuitionForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}