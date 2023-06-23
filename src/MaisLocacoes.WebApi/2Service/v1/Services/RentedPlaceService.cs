using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class RentedPlaceService : IRentedPlaceService
    {
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAddressService _addressService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RentedPlaceService(IRentedPlaceRepository rentedPlaceRepository,
            IProductRepository productRepository,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _rentedPlaceRepository = rentedPlaceRepository;
            _productRepository = productRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<RentedPlaceResponse> CreateRentedPlace(RentedPlaceRequest rentedPlaceRequest)
        {
            var existsproduct = await _productRepository.ProductExists(rentedPlaceRequest.ProductId);
            if (!existsproduct)
            {
                throw new HttpRequestException("Não existe esse produto", null, HttpStatusCode.BadRequest);
            }

            var rentedPlaceEntity = _mapper.Map<RentedPlaceEntity>(rentedPlaceRequest);

            rentedPlaceEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            rentedPlaceEntity = await _rentedPlaceRepository.CreateRentedPlace(rentedPlaceEntity);

            var rentedPlaceResponse = _mapper.Map<RentedPlaceResponse>(rentedPlaceEntity);

            return rentedPlaceResponse;
        }

        public async Task<RentedPlaceResponse> GetById(int id)
        {
            var rentedPlaceEntity = await _rentedPlaceRepository.GetById(id) ??
                throw new HttpRequestException("Local não encontrado", null, HttpStatusCode.NotFound);

            var rentedPlaceResponse = _mapper.Map<RentedPlaceResponse>(rentedPlaceEntity);

            return rentedPlaceResponse;
        }

        public async Task<bool> UpdateRentedPlace(RentedPlaceRequest rentedPlaceRequest, int id)
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

            rentedPlaceForUpdate.ProductId = rentedPlaceRequest.ProductId;
            rentedPlaceForUpdate.RentId = rentedPlaceRequest.RentId;
            rentedPlaceForUpdate.Latitude = rentedPlaceRequest.Latitude;
            rentedPlaceForUpdate.Longitude = rentedPlaceRequest.Longitude;
            rentedPlaceForUpdate.UpdatedAt = System.DateTime.UtcNow;
            rentedPlaceForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (!await _addressService.UpdateAddress(rentedPlaceRequest.Address, rentedPlaceForUpdate.AddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar o local", null, HttpStatusCode.InternalServerError);

            if (await _rentedPlaceRepository.UpdateRentedPlace(rentedPlaceForUpdate) > 0) return true;
            else return false;
        }
    }
}