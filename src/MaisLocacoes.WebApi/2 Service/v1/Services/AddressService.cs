using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Service.v1.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddressService(IAddressRepository addressRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateAddressResponse> CreateAddress(AddressRequest addressRequest)
        {
            var addressEntity = _mapper.Map<AddressEntity>(addressRequest);

            addressEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            addressEntity = await _addressRepository.CreateAddress(addressEntity);

            return _mapper.Map<CreateAddressResponse>(addressEntity);
        }

        public async Task<GetAddressResponse> GetById(int addressId)
        {
            var addressEntity = await _addressRepository.GetById(addressId);
            return _mapper.Map<GetAddressResponse>(addressEntity);
        }

        public async Task<bool> UpdateAddress(AddressRequest addressRequest, int id)
        {
            var addressForUpdate = await _addressRepository.GetById(id) ??
                throw new HttpRequestException("Endereço não encontrado", null, HttpStatusCode.NotFound);

            addressForUpdate.Cep = addressRequest.Cep;
            addressForUpdate.Street = addressRequest.Street;
            addressForUpdate.Number = addressRequest.Number;
            addressForUpdate.Complement = addressRequest.Complement;
            addressForUpdate.District = addressRequest.District;
            addressForUpdate.City = addressRequest.City;
            addressForUpdate.State = addressRequest.State;
            addressForUpdate.Country = addressRequest.Country;
            addressForUpdate.UpdatedAt = System.DateTime.UtcNow;
            addressForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _addressRepository.UpdateAddress(addressForUpdate) > 0) return true;
            else return false;
        }
    }
}