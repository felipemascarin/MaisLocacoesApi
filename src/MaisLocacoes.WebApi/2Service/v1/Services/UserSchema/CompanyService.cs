using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.UserSchema;
using MaisLocacoes.WebApi.Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;
using Service.v1.IServices.UserSchema;
using System.Net;

namespace Service.v1.Services.UserSchema
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyAddressService _companyAddressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _timeZone;
        private readonly string _email;

        public CompanyService(ICompanyRepository companyRepository,
            ICompanyAddressService companyAddressService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyRepository = companyRepository;
            _companyAddressService = companyAddressService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _timeZone = int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateCompanyResponse> CreateCompany(CreateCompanyRequest companyRequest)
        {
            var existsCompany = await _companyRepository.GetByCnpj(companyRequest.Cnpj);

            if (existsCompany != null)
                throw new HttpRequestException("Empresa já cadastrada", null, HttpStatusCode.BadRequest);

            existsCompany = await _companyRepository.GetByEmail(companyRequest.Email);
            if (existsCompany != null)
                throw new HttpRequestException("Email já cadastrado", null, HttpStatusCode.BadRequest);

            var companyAddressResponse = await _companyAddressService.CreateCompanyAddress(companyRequest.CompanyAddress);

            var companyEntity = _mapper.Map<CompanyEntity>(companyRequest);

            companyEntity.CompanyAddressId = companyAddressResponse.Id;
            companyEntity.CreatedBy = _email;

            companyEntity = await _companyRepository.CreateCompany(companyEntity);

            var companyResponse = _mapper.Map<CreateCompanyResponse>(companyEntity);

            return companyResponse;
        }

        public async Task<GetCompanyByCnpjResponse> GetCompanyByCnpj(string cnpj)
        {
            var companyEntity = await _companyRepository.GetByCnpj(cnpj) ??
                throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            var companyResponse = _mapper.Map<GetCompanyByCnpjResponse>(companyEntity);

            var timeZoneConverter = new TimeZoneConverter<GetCompanyByCnpjResponse>(_companyRepository, _httpContextAccessor);

            return await timeZoneConverter.ConvertToTimeZoneLocal(companyResponse);
        }

        public async Task<bool> UpdateCompany(UpdateCompanyRequest companyRequest, string cnpj)
        {
            var companyForUpdate = await _companyRepository.GetByCnpj(cnpj) ??
                throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            if (companyRequest.Cnpj != cnpj)
                    throw new HttpRequestException("Não é possível alterar o CNPJ de uma empresa cadastrada", null, HttpStatusCode.BadRequest);

            if (companyRequest.Email != companyForUpdate.Email)
            {
                var existsEmail = await _companyRepository.GetByEmail(companyRequest.Email);
                if (existsEmail != null)
                    throw new HttpRequestException("O Email novo já está cadastrado em outra empresa", null, HttpStatusCode.BadRequest);
            }

            companyForUpdate.CompanyName = companyRequest.CompanyName;
            companyForUpdate.StateRegister = companyRequest.StateRegister;
            companyForUpdate.FantasyName = companyRequest.FantasyName;
            companyForUpdate.Cel = companyRequest.Cel;
            companyForUpdate.Tel = companyRequest.Tel;
            companyForUpdate.Email = companyRequest.Email;
            companyForUpdate.Segment = companyRequest.Segment;
            companyForUpdate.PjDocumentUrl = companyRequest.PjDocumentUrl;
            companyForUpdate.AddressDocumentUrl = companyRequest.AddressDocumentUrl;
            companyForUpdate.LogoUrl = companyRequest.LogoUrl;
            companyForUpdate.NotifyDaysBefore = companyRequest.NotifyDaysBefore;
            companyForUpdate.Module = companyRequest.Module;
            companyForUpdate.TimeZone = companyRequest.TimeZone;
            companyForUpdate.UpdatedAt = System.DateTime.Now;
            companyForUpdate.UpdatedBy = _email;

            if (!await _companyAddressService.UpdateCompanyAddress(companyRequest.CompanyAddress, companyForUpdate.CompanyAddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar a empresa", null, HttpStatusCode.InternalServerError);

            if (await _companyRepository.UpdateCompany(companyForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, string cnpj)
        {
            var companyForUpdate = await _companyRepository.GetByCnpj(cnpj) ??
                throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            companyForUpdate.Status = status;
            companyForUpdate.UpdatedAt = System.DateTime.Now;
            companyForUpdate.UpdatedBy = _email;

            if (await _companyRepository.UpdateCompany(companyForUpdate) > 0) return true;
            else return false;
        }
    }
}