using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;

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
            await context.Set<ContractEntity>().AddAsync(contractEntity);
            context.SaveChanges();
            return contractEntity;
        }
        public async Task<ContractEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(c => c.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);
        }

        public async Task<IEnumerable<ContractEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(c => c.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .Where(c => c.Deleted == false).ToListAsync();
        }

        public async Task<ContractEntity> GetContractInfoByRentId(int rentId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ContractEntity>()
            .Include(c => c.Rent).ThenInclude(r => r.Address)
            .Include(c => c.Rent.Client).ThenInclude(c => c.Address)
            .Include(c => c.Rent.ProductTuitions).ThenInclude(p => p.Product).ThenInclude(p => p.ProductType)
            .Include(c => c.Rent.ProductTuitions).ThenInclude(p => p.Bills)
            .Where(c => c.Deleted == false).OrderByDescending(c => c.Version).FirstOrDefaultAsync();
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
            context.Set<ContractEntity>().Update(contractForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}
