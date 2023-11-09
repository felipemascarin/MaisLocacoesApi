using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using TimeZoneConverter;

namespace Repository.v1.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly PostgreSqlContext _context;
        private readonly TimeZoneInfo _timeZone;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillRepository(PostgreSqlContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
        }

        public async Task<BillEntity> CreateBill(BillEntity billEntity)
        {
            await _context.Bills.AddAsync(billEntity);
            _context.SaveChanges();
            return billEntity;
        }

        public async Task<BillEntity> GetById(int id) => await _context.Bills
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.AddressEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.ClientEntity)
            .ThenInclude(client => client.AddressEntity).FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);

        public async Task<BillEntity> GetForTaxInvoice(int id) => await _context.Bills
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.AddressEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.ClientEntity)
            .ThenInclude(client => client.AddressEntity)
            .FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);

        public async Task<IEnumerable<BillEntity>> GetByRentId(int rentId) => await _context.Bills
            .Include(b => b.RentEntity)
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Where(b => b.RentId == rentId && b.Deleted == false).OrderBy(b => b.DueDate).ToListAsync();

        public async Task<IEnumerable<BillEntity>> GetByProductTuitionId(int? productTuitionId) => await _context.Bills
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.AddressEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.ClientEntity)
            .ThenInclude(client => client.AddressEntity)
            .Where(b => b.ProductTuitionId == productTuitionId && b.Deleted == false).ToListAsync();

        public async Task<IEnumerable<BillEntity>> GetDuedBills(int notifyDaysBefore) => await _context.Bills
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(r => r.ClientEntity)
            .Where(b => b.DueDate <= (TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone))
            .AddDays(notifyDaysBefore) && b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.Status != BillStatus.BillStatusEnum.ElementAt(3) /*canceled*/ && b.Deleted == false)
            .OrderByDescending(b => b.DueDate).ToListAsync();

        public async Task<IEnumerable<BillEntity>> GetAllDebts() => await _context.Bills
            .Include(b => b.ProductTuitionEntity)
            .ThenInclude(p => p.ProductTypeEntity)
            .Include(b => b.RentEntity)
            .ThenInclude(r => r.ClientEntity)
            .Where(b => b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.ProductTuitionEntity.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5) /*returned*/ && b.Deleted == false).ToListAsync();

        public async Task<int> UpdateBill(BillEntity billForUpdate)
        {
            _context.Bills.Update(billForUpdate);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteBill(BillEntity billForDelete)
        {
            _context.Bills.Remove(billForDelete);
            return await _context.SaveChangesAsync();
        }
    }
}