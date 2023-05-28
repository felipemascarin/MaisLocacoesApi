using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class CompanyTuitionService : ICompanyTuitionService
    {
        private readonly ICompanyTuitionRepository _companyTuitionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CompanyTuitionService(ICompanyTuitionRepository companyTuitionRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _companyTuitionRepository = companyTuitionRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<CompanyTuitionResponse> CreateCompanyTuition(CompanyTuitionRequest companyTuitionRequest)
        {
            var companyTuitionEntity = _mapper.Map<CompanyTuitionEntity>(companyTuitionRequest);

            companyTuitionEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            companyTuitionEntity = await _companyTuitionRepository.CreateCompanyTuition(companyTuitionEntity);

            var companyTuitionResponse = _mapper.Map<CompanyTuitionResponse>(companyTuitionEntity);

            return companyTuitionResponse;
        }

        public async Task<CompanyTuitionResponse> GetById(int id)
        {
            var companyTuitionEntity = await _companyTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            var companyTuitionResponse = _mapper.Map<CompanyTuitionResponse>(companyTuitionEntity);

            return companyTuitionResponse;
        }

        public async Task<bool> UpdateCompanyTuition(CompanyTuitionRequest companyTuitionRequest, int id)
        {
            var companyTuitionForUpdate = await _companyTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            companyTuitionForUpdate.AsaasNumber = companyTuitionRequest.AsaasNumber;
            companyTuitionForUpdate.TuitionNumber = companyTuitionRequest.TuitionNumber;
            companyTuitionForUpdate.Value = companyTuitionRequest.Value;
            companyTuitionForUpdate.PayDate = companyTuitionRequest.PayDate;
            companyTuitionForUpdate.DueDate = companyTuitionRequest.DueDate;
            companyTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow;
            companyTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyTuitionRepository.UpdateCompanyTuition(companyTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var companyTuitionForDelete = await _companyTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            companyTuitionForDelete.Deleted = true;
            companyTuitionForDelete.UpdatedAt = System.DateTime.UtcNow;
            companyTuitionForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyTuitionRepository.UpdateCompanyTuition(companyTuitionForDelete) > 0) return true;
            else return false;
        }
    }
}