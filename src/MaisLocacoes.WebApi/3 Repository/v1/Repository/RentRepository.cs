using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class RentRepository : IRentRepository
    {
        private readonly PostgreSqlContext _context;

        public RentRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<RentEntity> GetById(int id) => await _context.Rents.Include(r => r.AddressEntity).FirstOrDefaultAsync(r => r.Id == id);
    }
}