using MaisLocacoes.WebApi.DataBase.Context;
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
            await context.CompanyWastes.AddAsync(companyWasteEntity);
            context.SaveChanges();
            return companyWasteEntity;
        }

        public async Task<CompanyWasteEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.CompanyWastes.FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);
        }

        public async Task<int> UpdateCompanyWaste(CompanyWasteEntity companyWasteForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.CompanyWastes.Update(companyWasteForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}