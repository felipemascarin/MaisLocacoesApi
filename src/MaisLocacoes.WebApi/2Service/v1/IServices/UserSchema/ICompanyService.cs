using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateCompany(CompanyRequest companyRequest);
        Task<CompanyResponse> GetByCnpj(string cnpj);
        Task<bool> UpdateCompany(CompanyRequest companyRequest, string cnpj);
        Task<bool> UpdateStatus(string status, string cnpj);
    }
}