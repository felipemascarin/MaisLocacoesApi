using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository.UserSchema
{
    public interface ICompanyRepository
    {
        Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity);
        Task<CompanyEntity> GetByCnpj(string cnpj);
        Task<CompanyEntity> GetByEmail(string email);
        Task<bool> CompanyExists(string cnpj);
        Task<int> UpdateCompany(CompanyEntity companyForUpdate);
    }
}