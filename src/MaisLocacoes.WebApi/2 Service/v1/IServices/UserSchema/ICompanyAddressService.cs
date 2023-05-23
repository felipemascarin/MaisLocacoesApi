using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.UserSchema;

namespace MaisLocacoes.WebApi.Service.v1.IServices.UserSchema
{
    public interface ICompanyAddressService
    {
        Task<CreateCompanyAddressResponse> CreateCompanyAddress(CompanyAddressRequest companyAddressRequest);
        Task<GetCompanyAddressResponse> GetById(int companyAddressId);
        
        Task<bool> UpdateCompanyAddress(CompanyAddressRequest companyAddressRequest, int id);
    }
}