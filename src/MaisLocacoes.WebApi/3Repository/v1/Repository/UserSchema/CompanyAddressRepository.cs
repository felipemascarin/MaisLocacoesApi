using MaisLocacoes.WebApi.DataBase.Context.Factory;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.Repository.v1.Repository.UserSchema
{
    public class CompanyAddressRepository : ICompanyAddressRepository
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreSqlContextFactory _contextFactory;
        private readonly string _databaseName;

        public CompanyAddressRepository(PostgreSqlContextFactory contextFactory, IConfiguration configuration)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;
            _databaseName = _configuration["MyPostgreSqlConnection:AdmDatabaseName"];
        }

        public async Task<CompanyAddressEntity> CreateCompanyAddress(CompanyAddressEntity companyAddressEntity)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            await context.CompaniesAddresses.AddAsync(companyAddressEntity);
            context.SaveChanges();
            return companyAddressEntity;
        }
        
        public async Task<CompanyAddressEntity> GetById(int companyAddressId)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            return await context.CompaniesAddresses.Where(a => a.Id == companyAddressId).FirstOrDefaultAsync();
        }
        
        public async Task<int> UpdateCompanyAddress(CompanyAddressEntity companyAddressForUpdate)
        {
            using var context = _contextFactory.CreateContext(_databaseName);
            context.CompaniesAddresses.Update(companyAddressForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}