using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BillService(IBillRepository billRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _billRepository = billRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}