using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;
        private readonly IProductTuitionService _productTuitionService;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OsService(IOsRepository osRepository,
            IProductTuitionService productTuitionService,
            IProductTuitionRepository productTuitionRepository,
            IProductRepository productRepository,
            IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _osRepository = osRepository;
            _productTuitionService = productTuitionService;
            _productTuitionRepository = productTuitionRepository;
            _productRepository = productRepository;
            _rentedPlaceRepository = rentedPlaceRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<OsResponse> CreateOs(OsRequest osRequest)
        {
            var existsRent = await _productTuitionRepository.ProductTuitionExists(osRequest.ProductTuitionId);
            if (!existsRent)
            {
                throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
            }

            var osEntity = _mapper.Map<OsEntity>(osRequest);

            osEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            osEntity = await _osRepository.CreateOs(osEntity);

            var osResponse = _mapper.Map<OsResponse>(osEntity);

            return osResponse;
        }

        public async Task<bool> CloseOs(int id, CloseOsRequest closeOsRequest)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status == OsStatus.OsStatusEnum.ElementAt(2))
                throw new HttpRequestException("Nota se serviço já cancelada", null, HttpStatusCode.NotFound);

            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var product = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = JwtManager.GetCpfByToken(_httpContextAccessor);
            os.Status = OsStatus.OsStatusEnum.ElementAt(2);
            os.FinalDateTime = DateTime.Now;
            os.DeliveryObservation = closeOsRequest.DeliveryObservation;
            os.UpdatedAt = DateTime.UtcNow;
            os.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            var rentedPlace = new RentedPlaceEntity();
            rentedPlace.ProductId = product.Id;
            rentedPlace.Latitude = closeOsRequest.Latitude;
            rentedPlace.Longitude = closeOsRequest.Longitude;
            rentedPlace.ArrivalDate = os.FinalDateTime;
            rentedPlace.ProductParts = productTuitionEntity.Parts;
            rentedPlace.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(0)) //entrega
            {
                rentedPlace.RentId = productTuitionEntity.RentId;
                rentedPlace.QgId = null;

                await _productTuitionService.RetainProduct(productTuitionEntity, product);

                productTuitionEntity.ProductCode = closeOsRequest.ProductCode;
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2);
                productTuitionEntity.UpdatedAt = DateTime.UtcNow;
                productTuitionEntity.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);
            }

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(1)) //retirada
            {
                rentedPlace.RentId = null;
                rentedPlace.QgId = closeOsRequest.QgId;

                await _productTuitionService.ReleaseProduct(productTuitionEntity, product);

                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5);
                productTuitionEntity.UpdatedAt = DateTime.UtcNow;
                productTuitionEntity.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);
            }

            await _rentedPlaceRepository.CreateRentedPlace(rentedPlace);

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);

            if (await _osRepository.UpdateOs(os) > 0) return true;
            else return false;
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

            if (osRequest.ProductTuitionId != osForUpdate.ProductTuitionId)
            {
                var existsRent = await _productTuitionRepository.ProductTuitionExists(osRequest.ProductTuitionId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
                }
            }

            osForUpdate.ProductTuitionId = osRequest.ProductTuitionId;
            osForUpdate.DeliveryCpf = osRequest.DeliveryCpf;
            osForUpdate.InitialDateTime = osRequest.InitialDateTime;
            osForUpdate.FinalDateTime = osRequest.FinalDateTime;
            osForUpdate.Type = osRequest.Type;
            osForUpdate.DeliveryObservation = osRequest.DeliveryObservation;
            osForUpdate.UpdatedAt = System.DateTime.Now;
            osForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var osForUpdate = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForUpdate.Status = status;
            osForUpdate.UpdatedAt = System.DateTime.Now;
            osForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var osForDelete = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForDelete.Deleted = true;
            osForDelete.UpdatedAt = System.DateTime.Now;
            osForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(osForDelete) > 0) return true;
            else return false;
        }
    }
}