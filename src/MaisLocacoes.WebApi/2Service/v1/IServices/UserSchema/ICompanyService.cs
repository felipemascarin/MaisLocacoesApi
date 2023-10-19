using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface ICompanyService
    {
        Task<CreateCompanyResponse> CreateCompany(CreateCompanyRequest companyRequest);
        Task<GetCompanyByCnpjResponse> GetCompanyByCnpj(string cnpj);
        Task<bool> UpdateCompany(UpdateCompanyRequest companyRequest, string cnpj);
        Task<bool> UpdateStatus(string status, string cnpj);
    }
}