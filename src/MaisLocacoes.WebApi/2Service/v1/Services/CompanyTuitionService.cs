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
    public class CompanyTuitionService : ICompanyTuitionService
    {
        private readonly ICompanyTuitionRepository _companyTuitionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public CompanyTuitionService(ICompanyTuitionRepository companyTuitionRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _companyTuitionRepository = companyTuitionRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateCompanyTuitionResponse> CreateCompanyTuition(CreateCompanyTuitionRequest companyTuitionRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            companyTuitionRequest = TimeZoneConverter<CreateCompanyTuitionRequest>.ConvertToTimeZoneLocal(companyTuitionRequest, _timeZone);

            var companyTuitionEntity = _mapper.Map<CompanyTuitionEntity>(companyTuitionRequest);

            companyTuitionEntity.CreatedBy = _email;
            companyTuitionEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            companyTuitionEntity = await _companyTuitionRepository.CreateCompanyTuition(companyTuitionEntity);

            var companyTuitionResponse = _mapper.Map<CreateCompanyTuitionResponse>(companyTuitionEntity);

            return companyTuitionResponse;
        }

        public async Task<GetCompanyTuitionByIdResponse> GetCompanyTuitionById(int id)
        {
            var companyTuitionEntity = await _companyTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            var companyTuitionResponse = _mapper.Map<GetCompanyTuitionByIdResponse>(companyTuitionEntity);

            return companyTuitionResponse;
        }

        public async Task UpdateCompanyTuition(UpdateCompanyTuitionRequest companyTuitionRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            companyTuitionRequest = TimeZoneConverter<UpdateCompanyTuitionRequest>.ConvertToTimeZoneLocal(companyTuitionRequest, _timeZone);

            var companyTuitionForUpdate = await _companyTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            companyTuitionForUpdate.AsaasNumber = companyTuitionRequest.AsaasNumber;
            companyTuitionForUpdate.TuitionNumber = companyTuitionRequest.TuitionNumber;
            companyTuitionForUpdate.Value = companyTuitionRequest.Value;
            companyTuitionForUpdate.PayDate = companyTuitionRequest.PayDate;
            companyTuitionForUpdate.DueDate = companyTuitionRequest.DueDate;
            companyTuitionForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            companyTuitionForUpdate.UpdatedBy = _email;

            await _companyTuitionRepository.UpdateCompanyTuition(companyTuitionForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var companyTuitionForDelete = await _companyTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            companyTuitionForDelete.Deleted = true;
            companyTuitionForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            companyTuitionForDelete.UpdatedBy = _email;

            await _companyTuitionRepository.UpdateCompanyTuition(companyTuitionForDelete);
        }
    }
}