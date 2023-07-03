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
    public class QgService : IQgService
    {
        private readonly IQgRepository _qgRepository;
        private readonly IAddressService _addressService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public QgService(IQgRepository qgRepository,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _qgRepository = qgRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<QgResponse> CreateQg(QgRequest qgRequest)
        {
            var addressResponse = await _addressService.CreateAddress(qgRequest.Address);

            var qgEntity = _mapper.Map<QgEntity>(qgRequest);

            qgEntity.AddressId = addressResponse.Id;
            qgEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            qgEntity = await _qgRepository.CreateQg(qgEntity);

            var qgResponse = _mapper.Map<QgResponse>(qgEntity);

            return qgResponse;
        }

        public async Task<QgResponse> GetById(int id)
        {
            var qgEntity = await _qgRepository.GetById(id) ??
                throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            var qgResponse = _mapper.Map<QgResponse>(qgEntity);

            return qgResponse;
        }

        public async Task<bool> UpdateQg(QgRequest qgRequest, int id)
        {
            var qgForUpdate = await _qgRepository.GetById(id) ??
                    throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            qgForUpdate.Description = qgRequest.Description;
            qgForUpdate.Latitude = qgRequest.Latitude;
            qgForUpdate.Longitude = qgRequest.Longitude;
            qgForUpdate.UpdatedAt = System.DateTime.Now;
            qgForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (!await _addressService.UpdateAddress(qgRequest.Address, qgForUpdate.AddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar o QG", null, HttpStatusCode.InternalServerError);

            if (await _qgRepository.UpdateQg(qgForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var qgForDelete = await _qgRepository.GetById(id) ??
                throw new HttpRequestException("QG da empresa não encontrado", null, HttpStatusCode.NotFound);

            qgForDelete.Deleted = true;
            qgForDelete.UpdatedAt = System.DateTime.Now;
            qgForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _qgRepository.UpdateQg(qgForDelete) > 0) return true;
            else return false;
        }
    }
}