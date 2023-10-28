using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository
{
    public class ContractRepository : IContractRepository
    {
        private readonly PostgreSqlContext _context;

        public ContractRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ContractEntity> CreateContract(ContractEntity contractEntity)
        {
            await _context.Contracts.AddAsync(contractEntity);
            _context.SaveChanges();
            return contractEntity;
        }
        public async Task<ContractEntity> GetById(int id) => await _context.Contracts
            .Include(c => c.RentEntity).ThenInclude(c => c.AddressEntity)
            .Include(c => c.RentEntity.ClientEntity).ThenInclude(c => c.AddressEntity)
            .FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);

        public async Task<IEnumerable<ContractEntity>> GetAll() => await _context.Contracts
            .Include(c => c.RentEntity).ThenInclude(c => c.AddressEntity)
            .Include(c => c.RentEntity.ClientEntity).ThenInclude(c => c.AddressEntity)
            .Where(s => s.Deleted == false).ToListAsync();

        public async Task<int> UpdateContract(ContractEntity contractForUpdate)
        {
            _context.Contracts.Update(contractForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}
