using Repository.v1.IRepository;
using Service.v1.IServices;

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
    }
}