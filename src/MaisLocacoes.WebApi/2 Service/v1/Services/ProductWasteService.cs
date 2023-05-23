using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class ProductWasteService : IProductWasteService
    {
        private readonly IProductWasteRepository _productWasteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductWasteService(IProductWasteRepository productWasteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productWasteRepository = productWasteRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}