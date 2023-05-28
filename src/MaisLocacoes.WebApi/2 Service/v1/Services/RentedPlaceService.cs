using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
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

        public async Task<RentedPlaceResponse> CreateRentedPlace(RentedPlaceRequest rentedPlaceRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<RentedPlaceResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateRentedPlace(RentedPlaceRequest rentedPlaceRequest, int id)
        {
            throw new NotImplementedException();
        }
    }
}