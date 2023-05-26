using MaisLocacoes.WebApi.Context;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
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
            await _context.SaveChangesAsync();
            return billEntity;
        }
    }
}