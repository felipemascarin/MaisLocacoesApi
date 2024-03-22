using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyTuitionRepository : ICompanyTuitionRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public CompanyTuitionRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<CompanyTuitionEntity> CreateCompanyTuition(CompanyTuitionEntity companyTuitionEntity)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyTuitionEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return companyTuitionEntity;
        }

        public async Task<CompanyTuitionEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<CompanyTuitionEntity>().FirstOrDefaultAsync(c => c.Id == id );
        }

        public async Task<int> UpdateCompanyTuition(CompanyTuitionEntity companyTuitionForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyTuitionForUpdate).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteCompanyTuition(CompanyTuitionEntity companyTuitionForDelete)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyTuitionForDelete).State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }
    }
}