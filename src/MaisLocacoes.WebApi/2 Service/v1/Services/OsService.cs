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
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OsService(IOsRepository osRepository,
            IRentRepository rentRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _osRepository = osRepository;
            _rentRepository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<OsResponse> CreateOs(OsRequest osRequest)
        {
            var existsRent = await _rentRepository.RentExists(osRequest.RentId);
            if (!existsRent)
            {
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            var osEntity = _mapper.Map<OsEntity>(osRequest);

            osEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            osEntity = await _osRepository.CreateOs(osEntity);

            var osResponse = _mapper.Map<OsResponse>(osEntity);

            return osResponse;
        }

        public async Task<OsResponse> GetById(int id)
        {
            var osEntity = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            var osResponse = _mapper.Map<OsResponse>(osEntity);

            return osResponse;
        }

        public async Task<bool> UpdateOs(OsRequest osRequest, int id)
        {
            var osForUpdate = await _osRepository.GetById(id) ??
               throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            if (osRequest.RentId != osForUpdate.RentId)
            {
                var existsRent = await _rentRepository.RentExists(osRequest.RentId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
                }
            }

            osForUpdate.RentId = osRequest.RentId;
            osForUpdate.DeliveryCpf = osRequest.DeliveryCpf;
            osForUpdate.RentId = osRequest.RentId;
            osForUpdate.InitialDateTime = osRequest.InitialDateTime;
            osForUpdate.FinalDateTime = osRequest.FinalDateTime;
            osForUpdate.Description = osRequest.Description;
            osForUpdate.DeliveryObservation = osRequest.DeliveryObservation;
            osForUpdate.UpdatedAt = System.DateTime.UtcNow;
            osForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var osForUpdate = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForUpdate.Status = status;
            osForUpdate.UpdatedAt = System.DateTime.UtcNow;
            osForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var osForDelete = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForDelete.Deleted = true;
            osForDelete.UpdatedAt = System.DateTime.UtcNow;
            osForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForDelete) > 0) return true;
            else return false;
        }
    }
}