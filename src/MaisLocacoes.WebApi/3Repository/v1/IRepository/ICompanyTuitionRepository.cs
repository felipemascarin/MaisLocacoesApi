using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface ICompanyTuitionRepository
    {
        Task<CompanyTuitionEntity> CreateCompanyTuition(CompanyTuitionEntity companyTuitionEntity);
        Task<CompanyTuitionEntity> GetById(int id);
        Task<int> UpdateCompanyTuition(CompanyTuitionEntity companyTuitionForUpdate);
    }
}