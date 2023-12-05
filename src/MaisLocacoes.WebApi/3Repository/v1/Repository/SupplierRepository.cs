using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public SupplierRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
            using var _context = _contextFactory.CreateContext();
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