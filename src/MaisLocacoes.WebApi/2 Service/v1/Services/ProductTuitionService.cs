using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class ProductTuitionService : IProductTuitionService
    {
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionRepository = productTuitionRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}