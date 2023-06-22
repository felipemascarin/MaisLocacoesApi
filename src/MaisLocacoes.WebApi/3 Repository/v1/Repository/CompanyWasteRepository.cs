using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyWasteRepository : ICompanyWasteRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyWasteRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<CompanyWasteEntity> CreateCompanyWaste(CompanyWasteEntity companyWasteEntity)
        {
            await _context.CompanyWastes.AddAsync(companyWasteEntity);
            _context.SaveChanges();
            return companyWasteEntity;
        }

        public async Task<CompanyWasteEntity> GetById(int id) => await _context.CompanyWastes.FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);

        public async Task<int> UpdateCompanyWaste(CompanyWasteEntity companyWasteForUpdate)
        {
            _context.CompanyWastes.Update(companyWasteForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}