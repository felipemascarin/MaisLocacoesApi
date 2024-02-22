using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.RentedPlace;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class RentedPlaceService : IRentedPlaceService
    {
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public RentedPlaceService(IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _rentedPlaceRepository = rentedPlaceRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateRentedPlaceResponse> CreateRentedPlace(CreateRentedPlaceRequest rentedPlaceRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            rentedPlaceRequest = TimeZoneConverter<CreateRentedPlaceRequest>.ConvertToTimeZoneLocal(rentedPlaceRequest, _timeZone);

            var rentedPlaceEntity = _mapper.Map<RentedPlaceEntity>(rentedPlaceRequest);

            rentedPlaceEntity.CreatedBy = _email;
            rentedPlaceEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

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

        public async Task UpdateRentedPlace(UpdateRentedPlaceRequest rentedPlaceRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            rentedPlaceRequest = TimeZoneConverter<UpdateRentedPlaceRequest>.ConvertToTimeZoneLocal(rentedPlaceRequest, _timeZone);

            var rentedPlaceForUpdate = await _rentedPlaceRepository.GetById(id) ??
                throw new HttpRequestException("Local não encontrado", null, HttpStatusCode.NotFound);

            rentedPlaceForUpdate.ProductId = rentedPlaceRequest.ProductId;
            rentedPlaceForUpdate.ProductTuitionId = rentedPlaceRequest.ProductTuitionId;
            rentedPlaceForUpdate.Latitude = rentedPlaceRequest.Latitude;
            rentedPlaceForUpdate.Longitude = rentedPlaceRequest.Longitude;
            rentedPlaceForUpdate.ArrivalDate = rentedPlaceRequest.ArrivalDate;
            rentedPlaceForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            rentedPlaceForUpdate.UpdatedBy = _email;

            await _rentedPlaceRepository.UpdateRentedPlace(rentedPlaceForUpdate);
        }
    }
}