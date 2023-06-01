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

        public async Task<RentEntity> GetById(int id) => await _context.Rents.Include(r => r.AddressEntity).FirstOrDefaultAsync(r => r.Id == id && r.Deleted == false);

        public async Task<bool> RentExists(int id) => await _context.Rents.AnyAsync(r => r.Id == id && r.Deleted == false);

        public async Task<IEnumerable<RentEntity>> GetRentsByPage(int items, int page, string query)
        {
            if (query == null)
                return await _context.Rents.Include(r => r.AddressEntity).Where(r => r.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
            else
                return await _context.Rents.Include(r => r.AddressEntity).Where(r => r.Deleted == false && (
                     r.Status.Contains(query) ||
                     r.ClientEntity.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.Cpf.Contains(query) ||
                     r.ClientEntity.Cnpj.Contains(query) ||
                     r.ClientEntity.CompanyName.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.StateRegister.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.FantasyName.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.Cel.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.Tel.ToLower().Contains(query.ToLower()) ||
                     r.ClientEntity.Email.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.Cep.Contains(query) ||
                     r.AddressEntity.Street.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.Number.Contains(query) ||
                     r.AddressEntity.Complement.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.District.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.City.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.State.ToLower().Contains(query.ToLower()) ||
                     r.AddressEntity.Country.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
        }

        public async Task<int> UpdateRent(RentEntity rentForUpdate)
        {
            _context.Rents.Update(rentForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}