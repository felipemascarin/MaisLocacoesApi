using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Os;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class OsService : IOsService
    {
        private readonly IOsRepository _osRepository;
        private readonly IQgRepository _qgRepository;
        private readonly IProductTuitionService _productTuitionService;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRentedPlaceRepository _rentedPlaceRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public OsService(IOsRepository osRepository,
            IQgRepository qgRepository,
            IProductTuitionService productTuitionService,
            IProductTuitionRepository productTuitionRepository,
            IProductRepository productRepository,
            IRentedPlaceRepository rentedPlaceRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _osRepository = osRepository;
            _qgRepository = qgRepository;
            _productTuitionService = productTuitionService;
            _productTuitionRepository = productTuitionRepository;
            _productRepository = productRepository;
            _rentedPlaceRepository = rentedPlaceRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateOsResponse> CreateOs(CreateOsRequest osRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            osRequest = TimeZoneConverter<CreateOsRequest>.ConvertToTimeZoneLocal(osRequest, _timeZone);

            var existsRent = await _productTuitionRepository.ProductTuitionExists(osRequest.ProductTuitionId);

            if (!existsRent)
                throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);

            var os = await _osRepository.GetByProductTuitionIdForCreate(osRequest.ProductTuitionId.Value, osRequest.Type);

            if (os != null)
                throw new HttpRequestException("Uma nota de serviço desse tipo já existe para esse produto.", null, HttpStatusCode.BadRequest);

            var osEntity = _mapper.Map<OsEntity>(osRequest);

            osEntity.CreatedBy = _email;
            osEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            osEntity = await _osRepository.CreateOs(osEntity);

            var osResponse = _mapper.Map<CreateOsResponse>(osEntity);

            return osResponse;
        }

        public async Task StartOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            //Se a nota for status Started, completed ou canceled, não pode ser iniciada
            if (os.Status != OsStatus.OsStatusEnum.ElementAt(0) /*waiting*/ && os.Status != OsStatus.OsStatusEnum.ElementAt(3) /*returned*/)
                throw new HttpRequestException("Essa nota de serviço não pode mais ser iniciada.", null, HttpStatusCode.NotFound);

            var productTuitionEntity = await _productTuitionRepository.GetById(os.ProductTuitionId) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(3) /*deliver*/;
            productTuitionEntity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionEntity.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);

            os.DeliveryCpf = JwtManager.GetCpfByToken(_httpContextAccessor);
            os.Status = OsStatus.OsStatusEnum.ElementAt(1) /*started*/;
            os.InitialDateTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.UpdatedBy = _email;

            await _osRepository.UpdateOs(os);
        }

        public async Task ReturnOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            //Somente notas iniciadas podem ser devolvidas
            if (os.Status != OsStatus.OsStatusEnum.ElementAt(1) /*started*/)
                throw new HttpRequestException("Essa nota de serviço não pode ser devolvida.", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = null;
            os.Status = OsStatus.OsStatusEnum.ElementAt(3) /*returned*/;
            os.InitialDateTime = null;
            os.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.UpdatedBy = _email;

            await _osRepository.UpdateOs(os);
        }

        public async Task CancelOs(int id)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            //Somente notas com status aguardando, iniciada ou completada podem ser canceladas
            if (os.Status == OsStatus.OsStatusEnum.ElementAt(2) /*completed*/ || os.Status == OsStatus.OsStatusEnum.ElementAt(4) /*canceled*/)
                throw new HttpRequestException("Essa nota de serviço não pode mais ser cancelada.", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = null;
            os.Status = OsStatus.OsStatusEnum.ElementAt(4) /*canceled*/;
            os.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.UpdatedBy = _email;

            await _osRepository.UpdateOs(os);
        }

        public async Task FinishOs(int id, FinishOsRequest finishOsRequest)
        {
            var os = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota se serviço não encontrada", null, HttpStatusCode.NotFound);

            if (os.Status != OsStatus.OsStatusEnum.ElementAt(1) /*started*/)
                throw new HttpRequestException("Não é possível finalizar uma nota de serviço não iniciada", null, HttpStatusCode.NotFound);

            os.DeliveryCpf = JwtManager.GetCpfByToken(_httpContextAccessor);
            os.Status = OsStatus.OsStatusEnum.ElementAt(2) /*completed*/;
            os.FinalDateTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.DeliveryObservation = finishOsRequest.DeliveryObservation;
            os.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            os.UpdatedBy = _email;

            var rentedPlace = new RentedPlaceEntity();
            rentedPlace.Latitude = finishOsRequest.Latitude.Value;
            rentedPlace.Longitude = finishOsRequest.Longitude.Value;
            rentedPlace.ArrivalDate = os.FinalDateTime;
            rentedPlace.CreatedBy = _email;

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(0)) //delivery
            {
                var productTuitionEntity = await _productTuitionRepository.GetById(finishOsRequest.ProductTuitionId.Value) ??
                    throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

                var product = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                    throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);                             

                //Se for um producttuition que esta sendo alterado o codigo do produto, então o produto novo é retido e o antigo liberado
                if (productTuitionEntity.ProductCode != finishOsRequest.ProductCode)
                {
                    await _productTuitionService.RetainProduct(productTuitionEntity, product);

                    if (productTuitionEntity.ProductCode != null)
                        await _productTuitionService.ReleaseProduct(productTuitionEntity, product);
                }

                //Se o produto tem várias partes, então cria um place com Id do producttuition, mas se for uma única parte cria um place com Id do próprio produto
                if (product.ProductType.IsManyParts == true)
                    rentedPlace.ProductTuitionId = productTuitionEntity.Id;
                else
                    rentedPlace.ProductId = product.Id;

                productTuitionEntity.ProductCode = finishOsRequest.ProductCode;
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2) /*delivered*/;
                productTuitionEntity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                productTuitionEntity.UpdatedBy = _email;

                await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);
            }

            if (os.Type == OsTypes.OsTypesEnum.ElementAt(1)) //withdraw
            {
                if (os.ProductTuitionId != finishOsRequest.ProductTuitionId)
                    throw new HttpRequestException("Informe o mesmo producttuitionId que está na nota para finalizar uma nota de retirada", null, HttpStatusCode.NotFound);

                var productTuitionEntity = await _productTuitionRepository.GetById(os.ProductTuitionId) ??
                    throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

                var product = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                    throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

                rentedPlace.ProductId = product.Id;

                await _productTuitionService.ReleaseProduct(productTuitionEntity, product);

                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5) /*returned*/;
                productTuitionEntity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                productTuitionEntity.UpdatedBy = _email;

                await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);

                _productTuitionService.FinishRentIfTheLast(productTuitionEntity);
            }

            await _rentedPlaceRepository.CreateRentedPlace(rentedPlace);

            await _osRepository.UpdateOs(os);
        }

        public async Task<GetOsByIdResponse> GetOsById(int id)
        {
            var osEntity = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            var osResponse = _mapper.Map<GetOsByIdResponse>(osEntity);

            return osResponse;
        }

        public async Task<IEnumerable<GetAllOsByStatusResponse>> GetAllOsByStatus(string status)
        {
            var osEntityList = await _osRepository.GetAllByStatus(status);

            return _mapper.Map<List<GetAllOsByStatusResponse>>(osEntityList); ;
        }

        public async Task UpdateOs(UpdateOsRequest osRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            osRequest = TimeZoneConverter<UpdateOsRequest>.ConvertToTimeZoneLocal(osRequest, _timeZone);

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

            osForUpdate.ProductTuitionId = osRequest.ProductTuitionId.Value;
            osForUpdate.DeliveryCpf = osRequest.DeliveryCpf;
            osForUpdate.InitialDateTime = osRequest.InitialDateTime;
            osForUpdate.FinalDateTime = osRequest.FinalDateTime;
            osForUpdate.Type = osRequest.Type;
            osForUpdate.DeliveryObservation = osRequest.DeliveryObservation;
            osForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            osForUpdate.UpdatedBy = _email;

            await _osRepository.UpdateOs(osForUpdate);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var osForUpdate = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForUpdate.Status = status;
            osForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            osForUpdate.UpdatedBy = _email;

            await _osRepository.UpdateOs(osForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var osForDelete = await _osRepository.GetById(id) ??
                throw new HttpRequestException("Nota de serviço não encontrada", null, HttpStatusCode.NotFound);

            osForDelete.Deleted = true;
            osForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            osForDelete.UpdatedBy = _email;

            await _osRepository.UpdateOs(osForDelete);
        }
    }
}