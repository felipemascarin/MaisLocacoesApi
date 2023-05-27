using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly PostgreSqlContext _context;

        public ClientRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ClientEntity> CreateClient(ClientEntity clientEntity)
        {
            await _context.Clients.AddAsync(clientEntity);
            await _context.SaveChangesAsync();
            return clientEntity;
        }

        public async Task<ClientEntity> GetByCpf(string cpf) => await _context.Clients.FirstOrDefaultAsync(c => c.Cpf == cpf && c.Deleted == false);
        
        public async Task<ClientEntity> GetByCnpj(string cnpj) => await _context.Clients.FirstOrDefaultAsync(c => c.Cnpj == cnpj && c.Deleted == false);

        public async Task<ClientEntity> GetById(int id) => await _context.Clients.Include(c => c.AddressEntity).FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);

        public async Task<bool> ClientExists(int id) => await _context.Clients.AnyAsync(c => c.Id == id && c.Deleted == false);

        public async Task<IEnumerable<ClientEntity>> GetClientsByPage(int items, int page, string query)
        {
            if (query == null)
            return await _context.Clients.Where(c => c.Deleted == false).Skip((page - 1) * items).Take(items).Include(c => c.AddressEntity).ToListAsync();
            else
            return await _context.Clients.Where(c => c.Deleted == false && (
                 c.Cpf.Contains(query) ||
                 c.Cnpj.Contains(query) ||
                 c.CompanyName.ToLower().Contains(query.ToLower()) ||
                 c.ClientName.ToLower().Contains(query.ToLower()) ||
                 c.StateRegister.ToLower().Contains(query.ToLower()) ||
                 c.FantasyName.ToLower().Contains(query.ToLower()) ||
                 c.Cel.ToLower().Contains(query.ToLower()) ||
                 c.Tel.ToLower().Contains(query.ToLower()) ||
                 c.Email.ToLower().Contains(query.ToLower())))
                 .Include(c => c.AddressEntity)
                 .Skip((page - 1) * items).Take(items).Include(c => c.AddressEntity).ToListAsync();
        }

        public async Task<IEnumerable<GetClientForRentDtoResponse>> GetClientsForRent() 
            => await _context.Clients
                .Where(c => c.Status == ClientStatus.ClientStatusEnum.ElementAt(0) && c.Deleted == false)
                .Select(c => new GetClientForRentDtoResponse 
                { 
                    Id = c.Id,
                    Cpf = c.Cpf,
                    ClientName = c.ClientName,
                    Cnpj = c.Cnpj,
                    FantasyName = c.FantasyName 
                }).ToListAsync();

        public async Task<int> UpdateClient(ClientEntity clientForUpdate)
        {
            _context.Clients.Update(clientForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}