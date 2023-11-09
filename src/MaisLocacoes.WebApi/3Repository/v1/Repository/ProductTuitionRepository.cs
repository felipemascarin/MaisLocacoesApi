using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using TimeZoneConverter;

namespace Repository.v1.Repository
{
    public class ProductTuitionRepository : IProductTuitionRepository
    {
        private readonly PostgreSqlContext _context;
        private readonly TimeZoneInfo _timeZone;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionRepository(PostgreSqlContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
        }

        public async Task<ProductTuitionEntity> CreateProductTuition(ProductTuitionEntity productTuitionEntity)
        {
            await _context.ProductTuitions.AddAsync(productTuitionEntity);
            _context.SaveChanges();
            return productTuitionEntity;
        }

        public async Task<ProductTuitionEntity> GetById(int id) => await _context.ProductTuitions.Include(p => p.RentEntity).Include(p => p.RentEntity.AddressEntity).Include(p => p.ProductTypeEntity).FirstOrDefaultAsync(p => p.Id == id && p.Deleted == false);
               
        public async Task<bool> ProductTuitionExists(int? id) => await _context.ProductTuitions.AnyAsync(p => p.Id == id && p.Deleted == false);

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByRentId(int rentId) => await _context.ProductTuitions
            .Include(p => p.ProductTypeEntity)
            .Include(p => p.ProductEntity)
            .Include(p => p.RentEntity)
            .ThenInclude(p => p.AddressEntity)
            .Include(p => p.RentEntity.ClientEntity)
            .ThenInclude(p => p.AddressEntity)
            .Where(p => p.RentId == rentId && p.Deleted == false).OrderBy(p => p.FinalDateTime).ToListAsync();

        public async Task<IEnumerable<ProductTuitionEntity>> GetAllByProductTypeCode(int productTypeId, string productCode) => await _context.ProductTuitions.Include(p => p.RentEntity).Include(p => p.RentEntity.AddressEntity).Where(p => p.ProductTypeId == productTypeId && p.ProductCode == productCode && p.Deleted == false).OrderBy(p => p.InitialDateTime).ToListAsync();
        
        public async Task<IEnumerable<ProductTuitionEntity>> GetAllToRemove() => await _context.ProductTuitions.Include(p => p.ProductTypeEntity).Include(p => p.RentEntity).Include(p => p.RentEntity.AddressEntity).Include(p => p.RentEntity.ClientEntity).Include(p => p.RentEntity.ClientEntity.AddressEntity).Where(p => p.FinalDateTime.Date <= (TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone)).Date && p.Deleted == false).ToListAsync();
        
        public async Task<bool> ProductTuitionExists(int rentId, int productTypeId, string productCode) => await _context.ProductTuitions.AnyAsync(p => p.RentId == rentId && p.ProductTypeId == productTypeId && p.ProductCode.ToLower() == productCode.ToLower() && p.Deleted == false);

        public async Task<int> UpdateProductTuition(ProductTuitionEntity productTuitionForUpdate)
        {
            _context.ProductTuitions.Update(productTuitionForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}