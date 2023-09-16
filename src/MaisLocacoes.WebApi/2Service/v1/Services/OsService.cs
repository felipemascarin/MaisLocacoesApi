using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetOsByStatusResponse;

namespace Service.v1.Services
{
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IQgRepository _qgRepository;
        private readonly IProductTuitionService _productTuitionService;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OsService(IOsRepository osRepository,
            IRentRepository rentRepository,
            IQgRepository qgRepository,
            IProductTuitionService productTuitionService,
            IProductTuitionRepository productTuitionRepository,
            IProductRepository productRepository,
            IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _osRepository = osRepository;
            _rentRepository = rentRepository;
            _qgRepository = qgRepository;
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
                throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);

            var os = await _osRepository.GetByProductTuitionIdForCreate(osRequest.ProductTuitionId, osRequest.Type);

            if (os != null)
                throw new HttpRequestException("Uma nota de serviço desse tipo já existe para esse produto.", null, HttpStatusCode.BadRequest);

            var osEntity = _mapper.Map<OsEntity>(osRequest);

            osEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            osEntity = await _osRepository.CreateOs(osEntity);

            var osResponse = _mapper.Map<OsResponse>(osEntity);

            return osResponse;
        }

        public async Task<bool> StartOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status != OsStatus.OsStatusEnum.ElementAt(0) && os.Status != OsStatus.OsStatusEnum.ElementAt(3))
                throw new HttpRequestException("Essa nota de serviço não pode mais ser iniciada.", null, HttpStatusCode.NotFound);

            var productTuitionEntity = await _productTuitionRepository.GetById(os.ProductTuitionId) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(3);
            productTuitionEntity.UpdatedAt = DateTime.Now;
            productTuitionEntity.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);

            os.DeliveryCpf = JwtManager.GetCpfByToken(_httpContextAccessor);
            os.Status = OsStatus.OsStatusEnum.ElementAt(1);
            os.InitialDateTime = DateTime.Now;
            os.UpdatedAt = DateTime.Now;
            os.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(os) > 0) return true;
            else return false;
        }

        public async Task<bool> ReturnOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status != OsStatus.OsStatusEnum.ElementAt(1))
                throw new HttpRequestException("Essa nota de serviço não pode ser devolvida.", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = null;
            os.Status = OsStatus.OsStatusEnum.ElementAt(3);
            os.InitialDateTime = null;
            os.UpdatedAt = DateTime.Now;
            os.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(os) > 0) return true;
            else return false;
        }

        public async Task<bool> CancelOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status == OsStatus.OsStatusEnum.ElementAt(2) || os.Status == OsStatus.OsStatusEnum.ElementAt(4))
                throw new HttpRequestException("Essa nota de serviço não pode mais ser cancelada.", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = null;
            os.Status = OsStatus.OsStatusEnum.ElementAt(4);
            os.UpdatedAt = DateTime.Now;
            os.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _osRepository.UpdateOs(os) > 0) return true;
            else return false;
        }

        public async Task<bool> FinishOs(int id, CloseOsRequest closeOsRequest)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status != OsStatus.OsStatusEnum.ElementAt(1))
                throw new HttpRequestException("Não é possível finalizar uma nota de serviço não iniciada", null, HttpStatusCode.NotFound);

            var productTuitionEntity = await _productTuitionRepository.GetById(closeOsRequest.ProductTuitionId) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = JwtManager.GetCpfByToken(_httpContextAccessor);
            os.Status = OsStatus.OsStatusEnum.ElementAt(2);
            os.FinalDateTime = DateTime.Now;
            os.DeliveryObservation = closeOsRequest.DeliveryObservation;
            os.UpdatedAt = DateTime.Now;
            os.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            var rentedPlace = new RentedPlaceEntity();
            rentedPlace.Latitude = closeOsRequest.Latitude;
            rentedPlace.Longitude = closeOsRequest.Longitude;
            rentedPlace.ArrivalDate = os.FinalDateTime;
            rentedPlace.ProductParts = productTuitionEntity.Parts;
            rentedPlace.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(0)) //entrega
            {
                if (string.IsNullOrEmpty(closeOsRequest.ProductCode))
                    throw new HttpRequestException("Código do produto é obrigatório para finalizar uma nota de entrega", null, HttpStatusCode.NotFound);

                var product = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                    throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

                await _productTuitionService.RetainProduct(productTuitionEntity, product);

                productTuitionEntity.ProductCode = closeOsRequest.ProductCode;
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2);
                productTuitionEntity.UpdatedAt = DateTime.Now;
                productTuitionEntity.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                rentedPlace.RentId = productTuitionEntity.RentId;
                rentedPlace.QgId = null;
                rentedPlace.ProductId = product.Id;
                rentedPlace.ProductParts = productTuitionEntity.Parts;
            }

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(1)) //retirada
            {
                var product = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                    throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

                if (closeOsRequest.QgId == null)
                    throw new HttpRequestException("Id do Qg é obrigatório para finalizar uma nota de retirada", null, HttpStatusCode.NotFound);

                var qg = await _qgRepository.GetById(closeOsRequest.QgId.Value) ??
                    throw new HttpRequestException("Qg não encontrado", null, HttpStatusCode.NotFound);

                product = await _productTuitionService.ReleaseProduct(productTuitionEntity, product);

                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5);
                productTuitionEntity.UpdatedAt = DateTime.Now;
                productTuitionEntity.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                rentedPlace.RentId = null;
                rentedPlace.QgId = qg.Id;
                rentedPlace.ProductId = product.Id;
                rentedPlace.ProductParts = product.Parts - product.RentedParts;

                _productTuitionService.FinishRentIfTheLast(productTuitionEntity);
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

        public async Task<IEnumerable<GetOsByStatusResponse>> GetAllByStatus(string status)
        {
            var osEntityList = await _osRepository.GetAllByStatus(status);

            var osRelationTuition = _mapper.Map<List<GetOsByStatusRelationTuition>>(osEntityList);

            var productTuitions = new List<GetProductTuitionRentProductTypeClientReponse>();

            foreach (var osEntity in osEntityList)
            {
                productTuitions = (await _productTuitionService.GetAllByRentId(osEntity.ProductTuitionEntity.RentId)).ToList();

                foreach (var os in osRelationTuition)
                {
                    if (os.ProductTuition == null)
                        os.ProductTuition = productTuitions.FirstOrDefault(p => p.Id == os.ProductTuitionId);
                }

                productTuitions.Clear();
            }

            return _mapper.Map<List<GetOsByStatusResponse>>(osRelationTuition); ;
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