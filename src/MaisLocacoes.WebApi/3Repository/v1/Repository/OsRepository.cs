using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class OsRepository : IOsRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public OsRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<OsEntity> CreateOs(OsEntity osEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(osEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return osEntity;
        }

        public async Task<OsEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<OsEntity>().FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OsEntity>> GetAllByStatus(string status)
        {
            using var context = _contextFactory.CreateContext();
            if (status == null)
                return await context.Set<OsEntity>()
                    .Include(o => o.ProductTuition).ThenInclude(p => p.ProductType)
                    .Include(o => o.ProductTuition.Rent).ThenInclude(r => r.Address)
                    .Include(o => o.ProductTuition.Rent.Client).ThenInclude(c => c.Address)
                    .Where(o => o.Status != OsStatus.OsStatusEnum.ElementAt(2) && o.Status != OsStatus.OsStatusEnum.ElementAt(4) /*canceled*/ ).ToListAsync();

            return await context.Set<OsEntity>()
                    .Include(o => o.ProductTuition).ThenInclude(p => p.ProductType)
                    .Include(o => o.ProductTuition.Rent).ThenInclude(r => r.Address)
                    .Include(o => o.ProductTuition.Rent.Client).ThenInclude(c => c.Address)
                    .Where(o => o.Status == status ).ToListAsync();
        }

        public async Task<OsEntity> GetByProductTuitionId(int productTuitionId, string type)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<OsEntity>().FirstOrDefaultAsync(o => o.ProductTuitionId == productTuitionId && o.Type == type );
        }

        public async Task<OsEntity> GetByProductTuitionIdForCreate(int productTuitionId, string type)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<OsEntity>().FirstOrDefaultAsync(o => o.ProductTuitionId == productTuitionId && o.Type == type && o.Status != OsStatus.OsStatusEnum.ElementAt(2) && o.Status != OsStatus.OsStatusEnum.ElementAt(4) );
        }

        public async Task<int> UpdateOs(OsEntity osForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(osForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteOs(OsEntity osForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(osForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}