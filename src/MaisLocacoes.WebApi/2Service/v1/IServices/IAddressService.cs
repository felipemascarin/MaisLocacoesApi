using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IAddressService
    {
        Task<AddressResponse> CreateAddress(AddressRequest addressRequest);
        Task<AddressResponse> GetById(int addressId);
        Task<bool> UpdateAddress(AddressRequest addressRequest, int id);
    }
}