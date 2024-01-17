using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using System.Data;

namespace Repository.v1.Repository.UserSchema
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public CompanyRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity)
        {
            using var context = _contextFactory.CreateAdmContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return companyEntity;
        }

        public async Task<CompanyEntity> GetByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Companies.Include(c => c.CompanyAddress).FirstOrDefaultAsync(c => c.Cnpj == cnpj);
        }

        public async Task<IEnumerable<CompanyEntity>> GetByEmail(string email)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Companies.Include(c => c.CompanyAddress).Where(c => c.Email.ToLower() == email.ToLower()).ToListAsync();
        }

        public async Task<bool> CompanyExists(string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.Companies.AnyAsync(c => c.Cnpj == cnpj);
        }

        public async Task<int> UpdateCompany(CompanyEntity companyForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> ReturnDatabases()
        {
            using var context = _contextFactory.CreateAdmContext();
            var databases = await context.Companies.Select(company => company.DataBase).ToListAsync();
            return databases;
        }

        public async Task<IEnumerable<string>> ReturnAllDatabaseNames()
        {
            using var context = _contextFactory.CreateAdmContext();

            var sql = "SELECT datname FROM pg_database;";

            var databaseNames = new List<string>();

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                await context.Database.OpenConnectionAsync();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        databaseNames.Add(result.GetString(0));
                    }
                }
            }

            return databaseNames;
        }
    }
}