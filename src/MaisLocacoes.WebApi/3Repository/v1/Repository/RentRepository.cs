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
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(rentEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return rentEntity;
        }

        public async Task<RentEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<RentEntity>()
            .Include(r => r.Address)
            .Include(r => r.Client).ThenInclude(c => c.Address)
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
            return await context.Set<RentEntity>().Include(r => r.Address).Where(r => r.ClientId == clientId && r.Deleted == false).OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<RentEntity>> GetRentsByPage(int items, int page, string query, string status)
        {
            using var context = _contextFactory.CreateContext();
            if (query == null)
            {
                if (status == null)
                {
                    return await context.Set<RentEntity>().Include(r => r.Address).Include(r => r.Client).Include(r => r.Client.Address)
                        .Where(r => r.Deleted == false).Skip((page - 1) * items).Take(items).ToListAsync();
                }
                else return await context.Set<RentEntity>().Include(r => r.Address).Include(r => r.Client).Include(r => r.Client.Address)
                        .Where(r => r.Deleted == false && r.Status.ToLower() == status.ToLower()).Skip((page - 1) * items).Take(items).ToListAsync();
            }
            else
                if (status == null)
            {
                return await context.Set<RentEntity>().Include(r => r.Address).Include(r => r.Client).Include(r => r.Client.Address)
                    .Where(r => r.Deleted == false && (
                     r.Status.Contains(query) ||
                     r.Client.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.Client.Cpf.Contains(query) ||
                     r.Client.Cnpj.Contains(query) ||
                     r.Client.CompanyName.ToLower().Contains(query.ToLower()) ||
                     r.Client.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.Client.StateRegister.ToLower().Contains(query.ToLower()) ||
                     r.Client.FantasyName.ToLower().Contains(query.ToLower()) ||
                     r.Client.Cel.ToLower().Contains(query.ToLower()) ||
                     r.Client.Tel.ToLower().Contains(query.ToLower()) ||
                     r.Client.Email.ToLower().Contains(query.ToLower()) ||
                     r.Address.Cep.Contains(query) ||
                     r.Address.Street.ToLower().Contains(query.ToLower()) ||
                     r.Address.Number.Contains(query) ||
                     r.Address.Complement.ToLower().Contains(query.ToLower()) ||
                     r.Address.District.ToLower().Contains(query.ToLower()) ||
                     r.Address.City.ToLower().Contains(query.ToLower()) ||
                     r.Address.State.ToLower().Contains(query.ToLower()) ||
                     r.Address.Country.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
            }
            else
            {
                return await context.Set<RentEntity>().Include(r => r.Address).Include(r => r.Client).Include(r => r.Client.Address)
                    .Where(r => r.Deleted == false && r.Status.ToLower() == status.ToLower() && (
                     r.Status.Contains(query) ||
                     r.Client.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.Client.Cpf.Contains(query) ||
                     r.Client.Cnpj.Contains(query) ||
                     r.Client.CompanyName.ToLower().Contains(query.ToLower()) ||
                     r.Client.ClientName.ToLower().Contains(query.ToLower()) ||
                     r.Client.StateRegister.ToLower().Contains(query.ToLower()) ||
                     r.Client.FantasyName.ToLower().Contains(query.ToLower()) ||
                     r.Client.Cel.ToLower().Contains(query.ToLower()) ||
                     r.Client.Tel.ToLower().Contains(query.ToLower()) ||
                     r.Client.Email.ToLower().Contains(query.ToLower()) ||
                     r.Address.Cep.Contains(query) ||
                     r.Address.Street.ToLower().Contains(query.ToLower()) ||
                     r.Address.Number.Contains(query) ||
                     r.Address.Complement.ToLower().Contains(query.ToLower()) ||
                     r.Address.District.ToLower().Contains(query.ToLower()) ||
                     r.Address.City.ToLower().Contains(query.ToLower()) ||
                     r.Address.State.ToLower().Contains(query.ToLower()) ||
                     r.Address.Country.ToLower().Contains(query.ToLower())))
                    .Skip((page - 1) * items).Take(items).ToListAsync();
            }
        }

        public async Task<int> UpdateRent(RentEntity rentForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(rentForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}