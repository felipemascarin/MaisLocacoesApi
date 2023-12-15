using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class RentRepository : IRentRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public RentRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<RentEntity> CreateRent(RentEntity rentEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Set<RentEntity>().AddAsync(rentEntity);
            context.SaveChanges();
            return rentEntity;
        }

        public async Task<RentEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<RentEntity>()
            .Include(r => r.AddressEntity)
            .Include(r => r.ClientEntity).ThenInclude(c => c.AddressEntity)
            .FirstOrDefaultAsync(r => r.Id == id && r.Deleted == false);
        }

        public async Task<bool> RentExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<RentEntity>().AnyAsync(r => r.Id == id && r.Deleted == false);
        }

        public async Task<IEnumerable<RentEntity>> GetAllByClientId(int clientId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<RentEntity>().Include(r => r.AddressEntity).Where(r => r.ClientId == clientId && r.Deleted == false).OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<RentEntity>> GetRentsByPage(int items, int page, string query, string status)
        {
            using var context = _contextFactory.CreateContext();
            if (query == null)
            {
                if (status == null)
                {
                    return await context.Set<RentEntity>().Include(r => r.AddressEntity).Include(r => r.ClientEntity).Include(r => r.ClientEntity.AddressEntity)
                        .Where(r => r.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
                }
                else return await context.Set<RentEntity>().Include(r => r.AddressEntity).Include(r => r.ClientEntity).Include(r => r.ClientEntity.AddressEntity)
                        .Where(r => r.Deleted == false && r.Status.ToLower() == status.ToLower()).Skip((page - 1) * items).Take(items).ToListAsync();
            }
            else
                if (status == null)
            {
                return await context.Set<RentEntity>().Include(r => r.AddressEntity).Include(r => r.ClientEntity).Include(r => r.ClientEntity.AddressEntity)
                    .Where(r => r.Deleted == false && (
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
            else
            {
                return await context.Set<RentEntity>().Include(r => r.AddressEntity).Include(r => r.ClientEntity).Include(r => r.ClientEntity.AddressEntity)
                    .Where(r => r.Deleted == false && r.Status.ToLower() == status.ToLower() && (
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
        }

        public async Task<int> UpdateRent(RentEntity rentForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<RentEntity>().Update(rentForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}