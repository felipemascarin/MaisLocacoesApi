using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
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

        public CompanyService(ICompanyRepository companyRepository,
            ICompanyAddressService companyAddressService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyRepository = companyRepository;
            _companyAddressService = companyAddressService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CompanyResponse> CreateCompany(CompanyRequest companyRequest)
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
            companyEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            companyEntity = await _companyRepository.CreateCompany(companyEntity);

            var companyResponse = _mapper.Map<CompanyResponse>(companyEntity);
            companyResponse.CompanyAddress = companyAddressResponse;

            return companyResponse;
        }

        public async Task<CompanyResponse> GetByCnpj(string cnpj)
        {
            var companyEntity = await _companyRepository.GetByCnpj(cnpj) ??
                throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            var companyAddressResponse = _mapper.Map<CompanyAddressResponse>(companyEntity.CompanyAddressEntity);

            var companyResponse = _mapper.Map<CompanyResponse>(companyEntity);

            companyResponse.CompanyAddress = companyAddressResponse;

            return companyResponse;
        }

        public async Task<bool> UpdateCompany(CompanyRequest companyRequest, string cnpj)
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
            companyForUpdate.UpdatedAt = System.DateTime.Now;
            companyForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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
            companyForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyRepository.UpdateCompany(companyForUpdate) > 0) return true;
            else return false;
        }
    }
}