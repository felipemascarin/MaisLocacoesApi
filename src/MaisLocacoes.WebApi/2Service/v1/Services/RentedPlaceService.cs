using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class RentedPlaceService : IRentedPlaceService
    {
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IQgRepository _qgRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly int _timeZone;
        private readonly string _email;

        public RentedPlaceService(IRentedPlaceRepository rentedPlaceRepository,
            IProductRepository productRepository,
            IRentRepository rentRepository,
            IQgRepository qgRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _rentedPlaceRepository = rentedPlaceRepository;
            _productRepository = productRepository;
            _rentRepository = rentRepository;
            _qgRepository = qgRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateRentedPlaceResponse> CreateRentedPlace(CreateRentedPlaceRequest rentedPlaceRequest)
        {
            var existsproduct = await _productRepository.ProductExists(rentedPlaceRequest.ProductId);
            if (!existsproduct)
                throw new HttpRequestException("Não existe esse produto", null, HttpStatusCode.BadRequest);

            if (rentedPlaceRequest.RentId != null)
            {
                var existsRent = await _rentRepository.RentExists(rentedPlaceRequest.RentId.Value);
                if (!existsRent)
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            if (rentedPlaceRequest.QgId != null)
            {
                var existsQg = await _qgRepository.QgExists(rentedPlaceRequest.QgId.Value);
                if (!existsQg)
                    throw new HttpRequestException("Não existe esse QG", null, HttpStatusCode.BadRequest);
            }

            var rentedPlaceEntity = _mapper.Map<RentedPlaceEntity>(rentedPlaceRequest);

            rentedPlaceEntity.CreatedBy = _email;

            rentedPlaceEntity = await _rentedPlaceRepository.CreateRentedPlace(rentedPlaceEntity);

            var rentedPlaceResponse = _mapper.Map<CreateRentedPlaceResponse>(rentedPlaceEntity);

            return rentedPlaceResponse;
        }

        public async Task<GetRentedPlaceByIdResponse> GetRentedPlaceById(int id)
        {
            var rentedPlaceEntity = await _rentedPlaceRepository.GetById(id) ??
                throw new HttpRequestException("Local não encontrado", null, HttpStatusCode.NotFound);

            var rentedPlaceResponse = _mapper.Map<GetRentedPlaceByIdResponse>(rentedPlaceEntity);

            return rentedPlaceResponse;
        }

        public async Task<bool> UpdateRentedPlace(UpdateRentedPlaceRequest rentedPlaceRequest, int id)
        {
            var rentedPlaceForUpdate = await _rentedPlaceRepository.GetById(id) ??
                throw new HttpRequestException("Local não encontrado", null, HttpStatusCode.NotFound);

            if (rentedPlaceRequest.ProductId != rentedPlaceForUpdate.ProductId)
            {
                var existsproduct = await _productRepository.ProductExists(rentedPlaceRequest.ProductId);
                if (!existsproduct)
                {
                    throw new HttpRequestException("Não existe esse produto", null, HttpStatusCode.BadRequest);
                }
            }

            if (rentedPlaceRequest.RentId != null)
            {
                var existsRent = await _rentRepository.RentExists(rentedPlaceRequest.RentId.Value);
                if (!existsRent)
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            if (rentedPlaceRequest.QgId != null)
            {
                var existsQg = await _qgRepository.QgExists(rentedPlaceRequest.QgId.Value);
                if (!existsQg)
                    throw new HttpRequestException("Não existe esse QG", null, HttpStatusCode.BadRequest);
            }

            rentedPlaceForUpdate.ProductId = rentedPlaceRequest.ProductId;
            rentedPlaceForUpdate.RentId = rentedPlaceRequest.RentId;
            rentedPlaceForUpdate.QgId = rentedPlaceRequest.QgId;
            rentedPlaceForUpdate.Latitude = rentedPlaceRequest.Latitude;
            rentedPlaceForUpdate.Longitude = rentedPlaceRequest.Longitude;
            rentedPlaceForUpdate.ProductParts = rentedPlaceRequest.ProductParts;
            rentedPlaceForUpdate.UpdatedAt = System.DateTime.Now;
            rentedPlaceForUpdate.UpdatedBy = _email;

            if (await _rentedPlaceRepository.UpdateRentedPlace(rentedPlaceForUpdate) > 0) return true;
            else return false;
        }
    }
}