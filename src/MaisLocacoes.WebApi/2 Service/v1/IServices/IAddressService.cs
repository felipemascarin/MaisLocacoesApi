using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IAddressService
    {
        Task<CreateAddressResponse> CreateAddress(AddressRequest addressRequest);
        Task<GetAddressResponse> GetById(int addressId);
        Task<bool> UpdateAddress(AddressRequest addressRequest, int id);
    }
}