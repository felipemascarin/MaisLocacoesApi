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
            await _context.SaveChangesAsync();
            return supplierEntity;
        }

        public async Task<SupplierEntity> GetById(int id) => await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id && s.Deleted == false);

        public async Task<int> UpdateSupplier(SupplierEntity supplierForUpdate)
        {
            _context.Suppliers.Update(supplierForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}