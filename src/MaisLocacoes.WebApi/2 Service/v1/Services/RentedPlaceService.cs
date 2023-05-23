using Repository.v1.IRepository;
using Service.v1.IServices;

namespace Service.v1.Services
{
    public class RentedPlaceService : IRentedPlaceService
    {
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentedPlaceService(IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentedPlaceRepository = rentedPlaceRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}