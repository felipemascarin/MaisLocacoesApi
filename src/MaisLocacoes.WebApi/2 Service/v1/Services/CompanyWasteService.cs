using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class CompanyWasteService : ICompanyWasteService
    {
        private readonly ICompanyWasteRepository _companyWasteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyWasteService(ICompanyWasteRepository companyWasteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyWasteRepository = companyWasteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CompanyWasteResponse> CreateCompanyWaste(CompanyWasteRequest companyWasteRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyWasteResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCompanyWaste(CompanyWasteRequest companyWasteRequest, int id)
        {
            throw new NotImplementedException();
        }

        public async async Task<bool> DeleteById(int id)
        {
            var companyWasteForDelete = await _companyWasteRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            companyWasteForDelete.Deleted = true;
            companyWasteForDelete.UpdatedAt = System.DateTime.UtcNow;
            companyWasteForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _companyWasteRepository.UpdateCompanyWaste(companyWasteForDelete) > 0) return true;
            else return false;
        }
    }
}