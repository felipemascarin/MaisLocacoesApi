using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;

namespace Service.v1.IServices.UserSchema
{
    public interface ICompanyService
    {
        Task<CreateCompanyResponse> CreateCompany(CompanyRequest companyRequest);
        Task<GetCompanyResponse> GetByCnpj(string cnpj);
    }
}