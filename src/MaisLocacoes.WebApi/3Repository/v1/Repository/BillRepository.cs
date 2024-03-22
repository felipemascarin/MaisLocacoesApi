using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            .ThenInclude(client => client.Address).FirstOrDefaultAsync(b => b.Id == id);
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
            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int?> GetTheLastInvoiceId()
        {
            using var context = _contextFactory.CreateContext();
            var invoiceId = await context.Set<BillEntity>().OrderByDescending(b => b.InvoiceId)
                                                 .Select(b => b.InvoiceId)
                                                 .FirstOrDefaultAsync();
            if (invoiceId != null)
                return invoiceId;
            else
                return 0;
        }

        public async Task<IEnumerable<BillEntity>> GetByRentId(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.Rent)
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Where(b => b.RentId == rentId).OrderBy(b => b.DueDate).ToListAsync();
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
            .Where(b => b.ProductTuitionId == productTuitionId).ToListAsync();
        }

        public async Task<IEnumerable<BillEntity>> GetDuedBills(int items, int page, int notifyDaysBefore, DateTime todayDate)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(r => r.Client)
            .Where(b => b.DueDate <= todayDate
            .AddDays(notifyDaysBefore) && b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.Status != BillStatus.BillStatusEnum.ElementAt(3) /*canceled*/)
            .OrderByDescending(b => b.DueDate).Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<IEnumerable<BillEntity>> GetAllDebts(int items, int page)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<BillEntity>()
            .Include(b => b.ProductTuition)
            .ThenInclude(p => p.ProductType)
            .Include(b => b.Rent)
            .ThenInclude(r => r.Client)
            .Where(b => b.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && b.ProductTuition.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5) /*returned*/)
            .Skip((page - 1) * items).Take(items).ToListAsync();
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
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(billForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}