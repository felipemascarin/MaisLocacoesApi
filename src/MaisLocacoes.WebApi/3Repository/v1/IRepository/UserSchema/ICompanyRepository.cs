using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository.UserSchema
{
    public interface ICompanyRepository
    {
        Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity);
        Task<IEnumerable<CompanyEntity>> GetAllCompany();
        Task<CompanyEntity> GetByCnpj(string cnpj);
        Task<IEnumerable<CompanyEntity>> GetByEmail(string email);
        Task<bool> CompanyExists(string cnpj);
        Task<int> UpdateCompany(CompanyEntity companyForUpdate);
        Task<IEnumerable<string>> ReturnDatabases();
        Task<IEnumerable<string>> ReturnAllDatabaseNames();
        Task<bool> CompanyNameExists(string companyName);
    }
}