using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
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

        public CompanyTuitionService(ICompanyTuitionRepository companyTuitionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyTuitionRepository = companyTuitionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<CompanyTuitionResponse> CreateCompanyTuition(CompanyTuitionRequest companyTuitionRequest)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyTuitionResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCompanyTuition(CompanyTuitionRequest companyTuitionRequest, int id)
        {
            throw new NotImplementedException();
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