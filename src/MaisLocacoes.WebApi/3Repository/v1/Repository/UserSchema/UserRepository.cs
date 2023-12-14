using MaisLocacoes.WebApi.DataBase.Context.Factory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;

namespace Repository.v1.Repository.UserSchema
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly string _databaseName;

        public UserRepository(PostgreSqlContextFactory contextFactory, IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
            _databaseName = _configuration["MyPostgreSqlConnection:AdmDatabaseName"];
        }

        public async Task<UserEntity> CreateUser(UserEntity userEntity)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            await context.Users.AddAsync(userEntity);
            context.SaveChanges();
            return userEntity;
        }

        public async Task<bool> UserHasToken(string token, string email, string cnpj)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            return await context.Users.Where(u => u.Email == email && u.Cnpj == cnpj).AnyAsync(u => u.LastToken == token);
        }
        public async Task<UserEntity> GetByEmail(string email, string cnpj)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity> GetByCpf(string cpf, string cnpj)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            return await context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);
        }

        public async Task<IEnumerable<UserEntity>> GetAllByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            return await context.Users.Where(u => u.Cnpj == cnpj).ToListAsync();
        }

        public async Task<bool> UserExists(string email, string cpf, string cnpj)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Users.AnyAsync(u => u.Email == email && u.Cnpj == cnpj || u.Cpf == cpf && u.Cnpj == cnpj);
        }

        public async Task<int> UpdateUser(UserEntity userForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Users.Update(userForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}