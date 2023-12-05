using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;

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
            using var context = _contextFactory.CreateContext();
            await context.CreateDatabase(companyEntity.Cnpj);
            await context.Companies.AddAsync(companyEntity);
            context.SaveChanges();
            return companyEntity;
        }

        public async Task<CompanyEntity> GetByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Companies.Include(c => c.CompanyAddressEntity).FirstOrDefaultAsync(c => c.Cnpj == cnpj);
        }

        public async Task<CompanyEntity> GetByEmail(string email)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Companies.Include(c => c.CompanyAddressEntity).FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> CompanyExists(string cnpj)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Companies.AnyAsync(c => c.Cnpj == cnpj);
        }
        public async Task<int> UpdateCompany(CompanyEntity companyForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Companies.Update(companyForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}