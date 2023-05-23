using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IAddressRepository
    {
        Task<AddressEntity> CreateAddress(AddressEntity addressEntity);
        Task<AddressEntity> GetById(int addressId);
        Task<int> UpdateAddress(AddressEntity addressForUpdate);
    }
}