using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillRepository(PostgreSqlContextFactory contextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory; 
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BillEntity> CreateBill(BillEntity billEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(billEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return billEntity;
        }

        public async Task<BillEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Address)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Client)
            .ThenInclude(client => client.Address).FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);
        }

        public async Task<BillEntity> GetForTaxInvoice(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Address)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Client)
            .ThenInclude(client => client.Address)
            .FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);
        }

        public async Task<IEnumerable<BillEntity>> GetByRentId(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.Rent)
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Where(b => b.RentId == rentId && b.Deleted == false).OrderBy(b => b.DueDate).ToListAsync();
        }

        public async Task<IEnumerable<BillEntity>> GetByProductTuitionId(int? productTuitionId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Address)
            .Include(b => b.Rent)
            .ThenInclude(rent => rent.Client)
            .ThenInclude(client => client.Address)
            .Where(b => b.ProductTuitionId == productTuitionId && b.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<BillEntity>> GetDuedBills(int notifyDaysBefore, DateTime todayDate)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(r => r.Client)
            .Where(b => b.DueDate <= todayDate
            .AddDays(notifyDaysBefore) && b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.Status != BillStatus.BillStatusEnum.ElementAt(3) /*canceled*/ && b.Deleted == false)
            .OrderByDescending(b => b.DueDate).ToListAsync();
        }

        public async Task<IEnumerable<BillEntity>> GetAllDebts()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(r => r.Client)
            .Where(b => b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.ProductTuition.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5) /*returned*/ && b.Deleted == false).ToListAsync();
        }

        public async Task<int> UpdateBill(BillEntity billForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(billForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteBill(BillEntity billForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<BillEntity>().Remove(billForDelete);
            return await context.SaveChangesAsync();
        }
    }
}