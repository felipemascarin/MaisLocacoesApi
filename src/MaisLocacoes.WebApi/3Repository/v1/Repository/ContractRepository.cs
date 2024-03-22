using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public ContractRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<ContractEntity> CreateContract(ContractEntity contractEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(contractEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return contractEntity;
        }
        public async Task<ContractEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(c => c.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ContractEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(c => c.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .ToListAsync();
        }

        public async Task<ContractEntity> GetContractInfoByRentId(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(r => r.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .Include(c => c.Rent.ProductTuitions).ThenInclude(p => p.Product).ThenInclude(p => p.ProductType)
            .Include(c => c.Rent.ProductTuitions).ThenInclude(p => p.Bills)
            .OrderByDescending(c => c.Version).FirstOrDefaultAsync();
        }

        public async Task<int> GetTheLastVersion(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return (await context.Set<ContractEntity>().OrderByDescending(c => c.Version).FirstOrDefaultAsync(c => c.RentId == rentId)).Version;
        }

        public async Task<ContractEntity> GetTheLastContract(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>().OrderByDescending(c => c.Version).FirstOrDefaultAsync(c => c.RentId == rentId);
        }

        public async Task<int> UpdateContract(ContractEntity contractForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(contractForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteContract(ContractEntity contractForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(contractForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}
