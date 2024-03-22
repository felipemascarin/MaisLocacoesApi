using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public ClientRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<ClientEntity> CreateClient(ClientEntity clientEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(clientEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return clientEntity;
        }

        public async Task<ClientEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>()
                .Include(c => c.Address)
                .Include(c => c.Rents)
                .FirstOrDefaultAsync(c => c.Id == id );
        }

        public async Task<ClientEntity> GetByIdDetails(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>().Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ClientEntity> GetByCpf(string cpf)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>().Include(c => c.Address).FirstOrDefaultAsync(c => (c.Cpf == cpf ) && (c.Cnpj == null || c.Cnpj == string.Empty));
        }

        public async Task<ClientEntity> GetByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>().Include(c => c.Address).FirstOrDefaultAsync(c => c.Cnpj == cnpj );
        }

        public async Task<bool> ClientExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>().AnyAsync(c => c.Id == id );
        }

        public async Task<IEnumerable<ClientEntity>> GetClientsByPage(int items, int page, string query)
        {
            using var context = _contextFactory.CreateContext();
            if (query == null)
            return await context.Set<ClientEntity>().Include(c => c.Address).Skip((page - 1) * items).Take(items).ToListAsync();
            else
            return await context.Set<ClientEntity>().Include(c => c.Address).Where(c => (
                 c.Cpf.Contains(query) ||
                 c.Cnpj.Contains(query) ||
                 c.CompanyName.ToLower().Contains(query.ToLower()) ||
                 c.ClientName.ToLower().Contains(query.ToLower()) ||
                 c.StateRegister.ToLower().Contains(query.ToLower()) ||
                 c.FantasyName.ToLower().Contains(query.ToLower()) ||
                 c.Cel.ToLower().Contains(query.ToLower()) ||
                 c.Tel.ToLower().Contains(query.ToLower()) ||
                 c.Email.ToLower().Contains(query.ToLower())))
                 .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<IEnumerable<GetClientForRentDtoResponse>> GetClientsForRent()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<ClientEntity>()
                .Where(c => c.Status == ClientStatus.ClientStatusEnum.ElementAt(0) )
                .Select(c => new GetClientForRentDtoResponse
                {
                    Id = c.Id,
                    Cpf = c.Cpf,
                    ClientName = c.ClientName,
                    Cnpj = c.Cnpj,
                    FantasyName = c.FantasyName
                }).ToListAsync();
        }

        public async Task<int> UpdateClient(ClientEntity clientForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(clientForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteClient(ClientEntity clientForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(clientForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}