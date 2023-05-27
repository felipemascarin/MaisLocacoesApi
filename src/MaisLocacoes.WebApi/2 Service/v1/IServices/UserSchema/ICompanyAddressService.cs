using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace MaisLocacoes.WebApi.Service.v1.IServices.UserSchema
{
    public interface ICompanyAddressService
    {
        Task<CompanyAddressResponse> CreateCompanyAddress(CompanyAddressRequest companyAddressRequest);
        Task<CompanyAddressResponse> GetById(int companyAddressId);
        Task<bool> UpdateCompanyAddress(CompanyAddressRequest companyAddressRequest, int id);
    }
}