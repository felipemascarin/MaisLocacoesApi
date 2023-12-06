using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.Repository.v1.Repository.UserSchema
{
    public class CompanyAddressRepository : ICompanyAddressRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyAddressRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<CompanyAddressEntity> CreateCompanyAddress(CompanyAddressEntity companyAddressEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.CompaniesAddresses.AddAsync(companyAddressEntity);
            context.SaveChanges();
            return companyAddressEntity;
        }

        public async Task<CompanyAddressEntity> GetById(int companyAddressId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.CompaniesAddresses.Where(a => a.Id == companyAddressId).FirstOrDefaultAsync();
        }
        
        public async Task<int> UpdateCompanyAddress(CompanyAddressEntity companyAddressForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.CompaniesAddresses.Update(companyAddressForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}