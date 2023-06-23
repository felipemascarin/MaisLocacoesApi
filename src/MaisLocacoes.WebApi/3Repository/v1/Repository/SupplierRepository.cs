using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly PostgreSqlContext _context;

        public SupplierRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<SupplierEntity> CreateSupplier(SupplierEntity supplierEntity)
        {
            await _context.Suppliers.AddAsync(supplierEntity);
            _context.SaveChanges();
            return supplierEntity;
        }

        public async Task<SupplierEntity> GetById(int id) => await _context.Suppliers.Include(c => c.AddressEntity).FirstOrDefaultAsync(s => s.Id == id && s.Deleted == false);

        public async Task<IEnumerable<SupplierEntity>> GetAll() => await _context.Suppliers.Include(c => c.AddressEntity).Where(s => s.Deleted == false).ToListAsync();

        public async Task<int> UpdateSupplier(SupplierEntity supplierForUpdate)
        {
            _context.Suppliers.Update(supplierForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}