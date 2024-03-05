using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Qg;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class QgService : IQgService
    {
        private readonly IQgRepository _qgRepository;
        private readonly IAddressService _addressService;
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public QgService(IQgRepository qgRepository,
            IAddressService addressService,
            IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _qgRepository = qgRepository;
            _addressService = addressService;
            _rentedPlaceRepository = rentedPlaceRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateQgResponse> CreateQg(CreateQgRequest qgRequest)
        {
            var addressResponse = await _addressService.CreateAddress(qgRequest.Address);
            var addressEntity = _mapper.Map<AddressEntity>(addressResponse);

            var rentedPlaceEntity = new RentedPlaceEntity()
            {
                Latitude = qgRequest.Latitude.Value,
                Longitude = qgRequest.Longitude.Value,
                CreatedBy = _email,
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone)
            };

            await _rentedPlaceRepository.CreateRentedPlace(rentedPlaceEntity);

            var qgEntity = _mapper.Map<QgEntity>(qgRequest);

            qgEntity.RentedPlaceId = rentedPlaceEntity.Id;
            qgEntity.RentedPlace = rentedPlaceEntity;
            qgEntity.AddressId = addressResponse.Id;
            qgEntity.Address = addressEntity;
            qgEntity.CreatedBy = _email;
            qgEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            qgEntity = await _qgRepository.CreateQg(qgEntity);

            var qgResponse = _mapper.Map<CreateQgResponse>(qgEntity);

            return qgResponse;
        }

        public async Task<GetQgByIdResponse> GetQgById(int id)
        {
            var qgEntity = await _qgRepository.GetById(id) ??
                throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            var qgResponse = _mapper.Map<GetQgByIdResponse>(qgEntity);

            return qgResponse;
        }

        public async Task<IEnumerable<GetAllQgsResponse>> GetAllQgs()
        {
            var qgEntity = await _qgRepository.GetAll();

            var qgResponse = _mapper.Map<IEnumerable<GetAllQgsResponse>>(qgEntity);

            return qgResponse;
        }

        public async Task UpdateQg(UpdateQgRequest qgRequest, int id)
        {
            var qgForUpdate = await _qgRepository.GetById(id) ??
                    throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            var rentedPlace = await _rentedPlaceRepository.GetById(qgForUpdate.RentedPlaceId) ??
                    throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            var updatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            qgForUpdate.Description = qgRequest.Description;
            qgForUpdate.UpdatedAt = updatedAt;
            qgForUpdate.UpdatedBy = _email;

            rentedPlace.Latitude = qgRequest.Latitude.Value;
            rentedPlace.Longitude = qgRequest.Longitude.Value;
            rentedPlace.UpdatedAt = updatedAt;
            rentedPlace.UpdatedBy = _email;

            await _addressService.UpdateAddress(qgRequest.Address, qgForUpdate.Address.Id);

            await _rentedPlaceRepository.UpdateRentedPlace(rentedPlace);

            await _qgRepository.UpdateQg(qgForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var qgForDelete = await _qgRepository.GetById(id) ??
                throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            qgForDelete.Deleted = true;
            qgForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            qgForDelete.UpdatedBy = _email;

            await _qgRepository.UpdateQg(qgForDelete);
        }
    }
}