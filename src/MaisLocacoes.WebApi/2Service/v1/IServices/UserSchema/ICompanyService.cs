using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.Company;

namespace Service.v1.IServices.UserSchema
{
    public interface ICompanyService
    {
        Task<CreateCompanyResponse> CreateCompany(CreateCompanyRequest companyRequest);
        Task<IEnumerable<GetAllCompanyResponse>> GetAllCompany();
        Task<GetCompanyByCnpjResponse> GetCompanyByCnpj(string cnpj);
        Task UpdateCompany(UpdateCompanyRequest companyRequest, string cnpj);
        Task UpdateStatus(string status, string cnpj);
    }
}