using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.Repository.v1.Repository.UserSchema
{
    public class CompanyAddressRepository : ICompanyAddressRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public CompanyAddressRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CompanyAddressEntity> CreateCompanyAddress(CompanyAddressEntity companyAddressEntity)
        {
            using var context = _contextFactory.CreateAdmContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyAddressEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return companyAddressEntity;
        }

        public async Task<CompanyAddressEntity> GetById(int companyAddressId)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.CompaniesAddresses.Where(a => a.Id == companyAddressId).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCompanyAddress(CompanyAddressEntity companyAddressForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyAddressForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}