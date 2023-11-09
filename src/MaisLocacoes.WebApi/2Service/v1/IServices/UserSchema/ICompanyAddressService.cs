using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.CompanyAddress;

namespace MaisLocacoes.WebApi.Service.v1.IServices.UserSchema
{
    public interface ICompanyAddressService
    {
        Task<CreateCompanyAddressResponse> CreateCompanyAddress(CreateCompanyAddressRequest companyAddressRequest);
        Task<GetCompanyAddressByIdResponse> GetCompanyAddressById(int companyAddressId);
        Task UpdateCompanyAddress(UpdateCompanyAddressRequest companyAddressRequest, int id);
    }
}