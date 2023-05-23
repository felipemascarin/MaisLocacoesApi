using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentService(IRentRepository rentRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentRepository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}