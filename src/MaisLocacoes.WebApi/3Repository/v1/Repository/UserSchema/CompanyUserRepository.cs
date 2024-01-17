using MaisLocacoes.WebApi._3Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi._3Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository.UserSchema
{
    public class CompanyUserRepository : ICompanyUserRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public CompanyUserRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<CompanyUserEntity> CreateCompanyUser(CompanyUserEntity companyUserEntity)
        {
            using var context = _contextFactory.CreateAdmContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.Entry(companyUserEntity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return companyUserEntity;
        }

        public async Task<IEnumerable<string>> GetCnpjListByEmail(string email)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.CompaniesUsers.Where(c => c.Email.ToLower() == email.ToLower()).Select(c => c.Cnpj).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetEmailListByCnpj(string cnpj)
        {
            using var context = _contextFactory.CreateAdmContext();
            return await context.CompaniesUsers.Where(c => c.Cnpj == cnpj).Select(c => c.Email).ToListAsync();
        }

        public async Task DeleteCompanyUser(CompanyUserEntity userForDelete)
        {
            using var context = _contextFactory.CreateAdmContext();
            context.CompaniesUsers.Remove(userForDelete);
            await context.SaveChangesAsync();
        }
    }
}
