using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository.UserSchema;
using System.Net;

namespace MaisLocacoes.WebApi.Service.v1.Services.UserSchema
{
    public class CompanyAddressService : ICompanyAddressService
    {
        private readonly ICompanyAddressRepository _companyAddressRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _timeZone;
        private readonly string _email;

        public CompanyAddressService(ICompanyAddressRepository companyAddressRepository,
            ICompanyRepository companyRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyAddressRepository = companyAddressRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateCompanyAddressResponse> CreateCompanyAddress(CreateCompanyAddressRequest companyAddressRequest)
        {
            var companyAddressEntity = _mapper.Map<CompanyAddressEntity>(companyAddressRequest);

            companyAddressEntity.CreatedBy = _email;
            companyAddressEntity.CreatedAt = DateTime.UtcNow + TimeSpan.FromHours(_timeZone);

            companyAddressEntity = await _companyAddressRepository.CreateCompanyAddress(companyAddressEntity);

            return _mapper.Map<CreateCompanyAddressResponse>(companyAddressEntity);
        }

        public async Task<CreateCompanyAddressResponse> GetById(int companyAddressId)
        {
            var CompanyAddressEntity = await _companyAddressRepository.GetById(companyAddressId) ??
                throw new HttpRequestException("Endereço da empresa não encontrado", null, HttpStatusCode.NotFound);

            return _mapper.Map<CreateCompanyAddressResponse>(CompanyAddressEntity);
        }

        public async Task<bool> UpdateCompanyAddress(UpdateCompanyAddressRequest companyAddressRequest, int id)
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
            companyAddressForUpdate.UpdatedAt = DateTime.UtcNow + TimeSpan.FromHours(_timeZone);
            companyAddressForUpdate.UpdatedBy = _email;

            if (await _companyAddressRepository.UpdateCompanyAddress(companyAddressForUpdate) > 0) return true;
            else return false;
        }

        public async Task<CompanyAddressEntity> TimeZoneConverter(CompanyAddressEntity companyAddressEntity)
        {
            var timeZoneConverter = new TimeZoneConverter<CompanyAddressEntity>(_companyRepository, _httpContextAccessor);

            companyAddressEntity = await timeZoneConverter.ConvertToTimeZoneLocal(companyAddressEntity);

            return companyAddressEntity;
        }
    }
}