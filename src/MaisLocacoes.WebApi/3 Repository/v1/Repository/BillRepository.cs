using MaisLocacoes.WebApi.Context;
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

        public async Task<int> UpdateBill(BillEntity billForUpdate)
        {
            _context.Bills.Update(billForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}