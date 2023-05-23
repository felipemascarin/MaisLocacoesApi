using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.IRepository;
using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using System.Net;

namespace MaisLocacoes.WebApi._3_Repository.v1.Repository
{
    public class DeletionsRepository : IDeletionsRepository
    {
        private readonly PostgreSqlContext _context;

        public DeletionsRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ClientsDeletions> CreateClientsDeletions(ClientsDeletions clientForDelete)
        {
            await _context.ClientsDeletions.AddAsync(clientForDelete);
            await _context.SaveChangesAsync();
            return clientForDelete;
        }

        public async Task<int> DeleteClient(ClientEntity clientForDelete)
        {
            _context.Clients.Remove(clientForDelete);
            return await _context.SaveChangesAsync();
        }
    }
}