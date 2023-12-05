using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
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
            await context.CompanyTuitions.AddAsync(companyTuitionEntity);
            context.SaveChanges();
            return companyTuitionEntity;
        }

        public async Task<CompanyTuitionEntity> GetById(int id) => await _context.CompanyTuitions.FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);

        public async Task<int> UpdateCompanyTuition(CompanyTuitionEntity companyTuitionForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.CompanyTuitions.Update(companyTuitionForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}