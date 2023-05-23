using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTypeService(IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productTypeRepository = productTypeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}