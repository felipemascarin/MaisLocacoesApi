using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly PostgreSqlContext _context;

        public BillRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<BillEntity> CreateBill(BillEntity billEntity)
        {
            await _context.Bills.AddAsync(billEntity);
            _context.SaveChanges();
            return billEntity;
        }

        public async Task<BillEntity> GetById(int id) => await _context.Bills.FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);

        public async Task<BillEntity> GetForTaxInvoice(int id) =>
        await _context.Bills
        .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.AddressEntity)
        .Include(b => b.RentEntity)
            .ThenInclude(rent => rent.ClientEntity)
                .ThenInclude(client => client.AddressEntity)
        .FirstOrDefaultAsync(b => b.Id == id && b.Deleted == false);

        public async Task<IEnumerable<BillEntity>> GetByRentId(int rentId) => await _context.Bills.Where(b => b.RentId == rentId && b.Deleted == false).OrderBy(b => b.DueDate).ToListAsync();

        public async Task<IEnumerable<BillEntity>> GetDuedBills(int notifyDaysBefore) => await _context.Bills.Include(b => b.RentEntity).Include(b => b.RentEntity.ClientEntity).Where(b => b.DueDate <= DateTime.Now.AddDays(notifyDaysBefore) && b.Status != BillStatus.BillStatusEnum.ElementAt(1) && b.Status != BillStatus.BillStatusEnum.ElementAt(3) && b.Deleted == false).OrderByDescending(b => b.DueDate).ToListAsync();

        public async Task<int> UpdateBill(BillEntity billForUpdate)
        {
            _context.Bills.Update(billForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}