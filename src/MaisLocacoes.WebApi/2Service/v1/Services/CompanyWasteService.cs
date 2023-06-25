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
    public class CompanyWasteService : ICompanyWasteService
    {
        private readonly ICompanyWasteRepository _companyWasteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CompanyWasteService(ICompanyWasteRepository companyWasteRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _companyWasteRepository = companyWasteRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<CompanyWasteResponse> CreateCompanyWaste(CompanyWasteRequest companyWasteRequest)
        {
            var companyWasteEntity = _mapper.Map<CompanyWasteEntity>(companyWasteRequest);

            companyWasteEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            companyWasteEntity = await _companyWasteRepository.CreateCompanyWaste(companyWasteEntity);

            var companyWasteResponse = _mapper.Map<CompanyWasteResponse>(companyWasteEntity);

            return companyWasteResponse;
        }

        public async Task<CompanyWasteResponse> GetById(int id)
        {
            var companyWasteEntity = await _companyWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            var companyWasteResponse = _mapper.Map<CompanyWasteResponse>(companyWasteEntity);

            return companyWasteResponse;
        }

        public async Task<bool> UpdateCompanyWaste(CompanyWasteRequest companyWasteRequest, int id)
        {
            var companyWasteForUpdate = await _companyWasteRepository.GetById(id) ??
               throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            companyWasteForUpdate.Description = companyWasteRequest.Description;
            companyWasteForUpdate.Value = companyWasteRequest.Value;
            companyWasteForUpdate.Date = companyWasteRequest.Date;
            companyWasteForUpdate.UpdatedAt = System.DateTime.Now;
            companyWasteForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyWasteRepository.UpdateCompanyWaste(companyWasteForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var companyWasteForDelete = await _companyWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto da empresa não encontrado", null, HttpStatusCode.NotFound);

            companyWasteForDelete.Deleted = true;
            companyWasteForDelete.UpdatedAt = System.DateTime.Now;
            companyWasteForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyWasteRepository.UpdateCompanyWaste(companyWasteForDelete) > 0) return true;
            else return false;
        }
    }
}