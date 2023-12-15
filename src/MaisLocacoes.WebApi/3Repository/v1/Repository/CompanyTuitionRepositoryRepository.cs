using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyTuitionRepositoryRepository : ICompanyTuitionRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public CompanyTuitionRepositoryRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
        }

        public async Task<CompanyTuitionEntity> CreateCompanyTuition(CompanyTuitionEntity companyTuitionEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Set<CompanyTuitionEntity>().AddAsync(companyTuitionEntity);
            context.SaveChanges();
            return companyTuitionEntity;
        }

        public async Task<CompanyTuitionEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<CompanyTuitionEntity>().FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);
        }

        public async Task<int> UpdateCompanyTuition(CompanyTuitionEntity companyTuitionForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<CompanyTuitionEntity>().Update(companyTuitionForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}