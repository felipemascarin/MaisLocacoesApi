using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public AddressService(IAddressRepository addressRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateAddressResponse> CreateAddress(CreateAddressRequest addressRequest)
        {
            var addressEntity = _mapper.Map<AddressEntity>(addressRequest);

            addressEntity.CreatedBy = _email;
            addressEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            addressEntity = await _addressRepository.CreateAddress(addressEntity);

            return _mapper.Map<CreateAddressResponse>(addressEntity);
        }

        public async Task<GetAddressByIdResponse> GetAddressById(int addressId)
        {
            var addressEntity = await _addressRepository.GetById(addressId);
            return _mapper.Map<GetAddressByIdResponse>(addressEntity);
        }

        public async Task UpdateAddress(UpdateAddressRequest addressRequest, int id)
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
            addressForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            addressForUpdate.UpdatedBy = _email;

            await _addressRepository.UpdateAddress(addressForUpdate);
        }
    }
}