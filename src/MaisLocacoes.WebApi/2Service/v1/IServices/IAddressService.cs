using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IAddressService
    {
        Task<CreateAddressResponse> CreateAddress(CreateAddressRequest addressRequest);
        Task<GetAddressByIdResponse> GetAddressById(int addressId);
        Task<bool> UpdateAddress(UpdateAddressRequest addressRequest, int id);
    }
}