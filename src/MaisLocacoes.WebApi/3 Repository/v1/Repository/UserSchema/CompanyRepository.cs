using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;

namespace Repository.v1.Repository.UserSchema

    public class CompanyRepository : ICompanyRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity)
        {
            await _context.Companies.AddAsync(companyEntity);
            await _context.SaveChangesAsync();
            _context.CreateSchema(companyEntity.Cnpj);
            return companyEntity;
        }

        public async Task<CompanyEntity> GetByCnpj(string cnpj) => await _context.Companies.Include(c => c.CompanyAddressEntity).FirstOrDefaultAsync(c => c.Cnpj == cnpj);

        public async Task<CompanyEntity> GetByEmail(string email) => await _context.Companies.Include(c => c.CompanyAddressEntity).FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

        public async Task<int> UpdateCompany(CompanyEntity companyForUpdate)
        {
            _context.Companies.Update(companyForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}