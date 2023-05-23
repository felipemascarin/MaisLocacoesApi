using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierService(ISupplierRepository supplierRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _supplierRepository = supplierRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}