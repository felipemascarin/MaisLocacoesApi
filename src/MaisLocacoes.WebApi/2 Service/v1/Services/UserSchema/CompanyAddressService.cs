using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Repository.v1.IRepository.UserSchema;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using System.Net;

namespace MaisLocacoes.WebApi.Service.v1.Services.UserSchema
{
    public class CompanyAddressService : ICompanyAddressService
    {
        private readonly ICompanyAddressRepository _companyAddressRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyAddressService(ICompanyAddressRepository companyAddressRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyAddressRepository = companyAddressRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CompanyAddressResponse> CreateCompanyAddress(CompanyAddressRequest companyAddressRequest)
        {
            var companyAddressEntity = _mapper.Map<CompanyAddressEntity>(companyAddressRequest);

            companyAddressEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            companyAddressEntity = await _companyAddressRepository.CreateCompanyAddress(companyAddressEntity);

            return _mapper.Map<CompanyAddressResponse>(companyAddressEntity);
        }

        public async Task<CompanyAddressResponse> GetById(int companyAddressId)
        {
            var CompanyAddressEntity = await _companyAddressRepository.GetById(companyAddressId) ??
                throw new HttpRequestException("Endereço da empresa não encontrado", null, HttpStatusCode.NotFound);

            return _mapper.Map<CompanyAddressResponse>(CompanyAddressEntity);
        }

        public async Task<bool> UpdateCompanyAddress(CompanyAddressRequest companyAddressRequest, int id)
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
            companyAddressForUpdate.UpdatedAt = System.DateTime.UtcNow;
            companyAddressForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyAddressRepository.UpdateCompanyAddress(companyAddressForUpdate) > 0) return true;
            else return false;
        }
    }
}