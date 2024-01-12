using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
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
        }

        public async Task<SupplierEntity> CreateSupplier(SupplierEntity supplierEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(supplierEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return supplierEntity;
        }

        public async Task<SupplierEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<SupplierEntity>().Include(c => c.Address).FirstOrDefaultAsync(s => s.Id == id && s.Deleted == false);
        }

        public async Task<IEnumerable<SupplierEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<SupplierEntity>().Include(c => c.Address).Where(s => s.Deleted == false).ToListAsync();
        }

        public async Task<int> UpdateSupplier(SupplierEntity supplierForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(supplierForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}