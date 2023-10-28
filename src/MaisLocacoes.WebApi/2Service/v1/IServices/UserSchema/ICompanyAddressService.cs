using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;

namespace MaisLocacoes.WebApi.Service.v1.IServices.UserSchema
{
    public interface ICompanyAddressService
    {
        Task<CreateCompanyAddressResponse> CreateCompanyAddress(CreateCompanyAddressRequest companyAddressRequest);
        Task<CreateCompanyAddressResponse> GetById(int companyAddressId);
        Task UpdateCompanyAddress(UpdateCompanyAddressRequest companyAddressRequest, int id);
    }
}