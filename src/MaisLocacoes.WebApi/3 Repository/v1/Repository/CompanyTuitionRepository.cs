using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyTuitionRepository : ICompanyTuitionRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyTuitionRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<CompanyTuitionEntity> CreateCompanyTuition(CompanyTuitionEntity companyTuitionEntity)
        {
            await _context.CompanyTuitions.AddAsync(companyTuitionEntity);
            await _context.SaveChangesAsync();
            return companyTuitionEntity;
        }

        public async Task<CompanyTuitionEntity> GetById(int id) => await _context.CompanyTuitions.FirstOrDefaultAsync(c => c.Id == id && c.Deleted == false);

        public async Task<int> UpdateCompanyTuition(CompanyTuitionEntity companyTuitionForUpdate)
        {
            _context.CompanyTuitions.Update(companyTuitionForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}