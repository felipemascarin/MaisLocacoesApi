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

        public async Task<RentEntity> CreateRent(RentEntity rentEntity)
        {
            await _context.Rents.AddAsync(rentEntity);
            await _context.SaveChangesAsync();
            return rentEntity;
        }

        public async Task<RentEntity> GetById(int id) => await _context.Rents.Include(r => r.AddressEntity).FirstOrDefaultAsync(r => r.Id == id);

        public async Task<bool> RentExists(int id) => await _context.Rents.AnyAsync(r => r.Id == id);
    }
}