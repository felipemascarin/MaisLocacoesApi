using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface ICompanyWasteRepository
    {
        Task<CompanyWasteEntity> CreateCompanyWaste(CompanyWasteEntity companyWasteEntity);
        Task<CompanyWasteEntity> GetById(int id);
        Task<int> UpdateCompanyWaste(CompanyWasteEntity companyWasteForUpdate);
        Task<int> DeleteCompanyWaste(CompanyWasteEntity companyWasteForDelete);
    }
}