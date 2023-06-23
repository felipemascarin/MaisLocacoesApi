using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema
{
    public interface ICompanyAddressRepository
    {
        Task<CompanyAddressEntity> CreateCompanyAddress(CompanyAddressEntity companyAddressEntity);
        Task<CompanyAddressEntity> GetById(int companyAddressId);
        Task<int> UpdateCompanyAddress(CompanyAddressEntity companyAddressForUpdate);
    }
}