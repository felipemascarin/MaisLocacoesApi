using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyWasteRepository : ICompanyWasteRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public CompanyWasteRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<CompanyWasteEntity> CreateCompanyWaste(CompanyWasteEntity companyWasteEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyWasteEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return companyWasteEntity;
        }

        public async Task<CompanyWasteEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<CompanyWasteEntity>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> UpdateCompanyWaste(CompanyWasteEntity companyWasteForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyWasteForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteCompanyWaste(CompanyWasteEntity companyWasteForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyWasteForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}