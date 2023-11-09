using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema.CompanyAddress;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using System.Net;
using TimeZoneConverter;

namespace MaisLocacoes.WebApi.Service.v1.Services.UserSchema
{
    public class CompanyAddressService : ICompanyAddressService
    {
        private readonly ICompanyAddressRepository _companyAddressRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public CompanyAddressService(ICompanyAddressRepository companyAddressRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyAddressRepository = companyAddressRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateCompanyAddressResponse> CreateCompanyAddress(CreateCompanyAddressRequest companyAddressRequest)
        {
            var companyAddressEntity = _mapper.Map<CompanyAddressEntity>(companyAddressRequest);

            companyAddressEntity.CreatedBy = _email;
            companyAddressEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            companyAddressEntity = await _companyAddressRepository.CreateCompanyAddress(companyAddressEntity);

            return _mapper.Map<CreateCompanyAddressResponse>(companyAddressEntity);
        }

        public async Task<GetCompanyAddressByIdResponse> GetCompanyAddressById(int companyAddressId) 
        {
            var CompanyAddressEntity = await _companyAddressRepository.GetById(companyAddressId) ??
                throw new HttpRequestException("Endereço da empresa não encontrado", null, HttpStatusCode.NotFound);

            return _mapper.Map<GetCompanyAddressByIdResponse>(CompanyAddressEntity);
        }

        public async Task UpdateCompanyAddress(UpdateCompanyAddressRequest companyAddressRequest, int id)
        {
            var companyAddressForUpdate = await _companyAddressRepository.GetById(id) ??
                throw new HttpRequestException("Endereço da empresa não encontrado", null, HttpStatusCode.NotFound);

            companyAddressForUpdate.Cep = companyAddressRequest.Cep;
            companyAddressForUpdate.Street = companyAddressRequest.Street;
            companyAddressForUpdate.Number = companyAddressRequest.Number;
            companyAddressForUpdate.Complement = companyAddressRequest.Complement;
            companyAddressForUpdate.District = companyAddressRequest.District;
            companyAddressForUpdate.City = companyAddressRequest.City;
            companyAddressForUpdate.State = companyAddressRequest.State;
            companyAddressForUpdate.Country = companyAddressRequest.Country;
            companyAddressForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            companyAddressForUpdate.UpdatedBy = _email;

            await _companyAddressRepository.UpdateCompanyAddress(companyAddressForUpdate);
        }
    }
}