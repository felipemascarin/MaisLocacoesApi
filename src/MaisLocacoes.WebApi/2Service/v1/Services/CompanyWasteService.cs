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
    public class CompanyWasteService : ICompanyWasteService
    {
        private readonly ICompanyWasteRepository _companyWasteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public CompanyWasteService(ICompanyWasteRepository companyWasteRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _companyWasteRepository = companyWasteRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateCompanyWasteResponse> CreateCompanyWaste(CreateCompanyWasteRequest companyWasteRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            companyWasteRequest = TimeZoneConverter<CreateCompanyWasteRequest>.ConvertToTimeZoneLocal(companyWasteRequest, _timeZone);

            var companyWasteEntity = _mapper.Map<CompanyWasteEntity>(companyWasteRequest);

            companyWasteEntity.CreatedBy = _email;
            companyWasteEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            companyWasteEntity = await _companyWasteRepository.CreateCompanyWaste(companyWasteEntity);

            var companyWasteResponse = _mapper.Map<CreateCompanyWasteResponse>(companyWasteEntity);

            return companyWasteResponse;
        }

        public async Task<GetCompanyWasteByIdResponse> GetCompanyWasteById(int id)
        {
            var companyWasteEntity = await _companyWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            var companyWasteResponse = _mapper.Map<GetCompanyWasteByIdResponse>(companyWasteEntity);

            return companyWasteResponse;
        }

        public async Task<bool> UpdateCompanyWaste(UpdateCompanyWasteRequest companyWasteRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            companyWasteRequest = TimeZoneConverter<UpdateCompanyWasteRequest>.ConvertToTimeZoneLocal(companyWasteRequest, _timeZone);

            var companyWasteForUpdate = await _companyWasteRepository.GetById(id) ??
               throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            companyWasteForUpdate.Description = companyWasteRequest.Description;
            companyWasteForUpdate.Value = companyWasteRequest.Value;
            companyWasteForUpdate.Date = companyWasteRequest.Date;
            companyWasteForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            companyWasteForUpdate.UpdatedBy = _email;

            if (await _companyWasteRepository.UpdateCompanyWaste(companyWasteForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var companyWasteForDelete = await _companyWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            companyWasteForDelete.Deleted = true;
            companyWasteForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            companyWasteForDelete.UpdatedBy = _email;

            if (await _companyWasteRepository.UpdateCompanyWaste(companyWasteForDelete) > 0) return true;
            else return false;
        }
    }
}