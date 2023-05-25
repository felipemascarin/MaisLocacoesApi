using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository.UserSchema
{
    public interface ICompanyRepository
    {
        Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity);
        Task<CompanyEntity> GetByCnpj(string cnpj);
        Task<CompanyEntity> GetByEmail(string email);
        Task<int> UpdateCompany(CompanyEntity companyForUpdate);
    }
}