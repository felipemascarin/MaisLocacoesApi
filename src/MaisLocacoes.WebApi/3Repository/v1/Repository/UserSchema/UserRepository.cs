using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;

namespace Repository.v1.Repository.UserSchema
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public UserRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<UserEntity> CreateUser(UserEntity userEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(userEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return userEntity;
        }

        public async Task<bool> UserHasToken(string token, string email, string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Users.Where(u => u.Email == email && u.Cnpj == cnpj).AnyAsync(u => u.LastToken == token);
        }
        public async Task<UserEntity> GetByEmail(string email, string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity> GetByCpf(string cpf, string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);
        }

        public async Task<IEnumerable<UserEntity>> GetAllByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Users.Where(u => u.Cnpj == cnpj).ToListAsync();
        }

        public async Task<bool> UserExists(string email, string cpf, string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Users.AnyAsync(u => u.Email == email && u.Cnpj == cnpj || u.Cpf == cpf && u.Cnpj == cnpj);
        }

        public async Task<int> UpdateUser(UserEntity userForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(userForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}