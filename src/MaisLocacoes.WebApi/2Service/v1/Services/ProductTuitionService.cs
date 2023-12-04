using AutoMapper;
using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Address;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Client;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductTuition.GetAllProductTuitionByProductIdResponse;

namespace Service.v1.Services
{
    public class ProductTuitionService : IProductTuitionService
    {
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IBillRepository _billRepository;
        private readonly IBillService _billService;
        private readonly IOsRepository _osRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IRentRepository rentRepository,
            IBillRepository billRepository,
            IBillService billService,
            IOsRepository osRepository,
            IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IContractRepository contractRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTuitionRepository = productTuitionRepository;
            _rentRepository = rentRepository;
            _billRepository = billRepository;
            _billService = billService;
            _osRepository = osRepository;
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _contractRepository = contractRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductTuitionResponse> CreateProductTuition(CreateProductTuitionRequest productTuitionRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productTuitionRequest = TimeZoneConverter<CreateProductTuitionRequest>.ConvertToTimeZoneLocal(productTuitionRequest, _timeZone);

            var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId.Value);
            if (!existsRent)
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);

            var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId.Value);
            if (!existsProductType)
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);

            var productEntity = new ProductEntity();

            //Se estiver já sendo cadastrado um produto, verifica se já existe esse produto nessa locação e retém o produto no estoque
            if (!string.IsNullOrEmpty(productTuitionRequest.ProductCode))
            {
                var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId.Value, productTuitionRequest.ProductTypeId.Value, productTuitionRequest.ProductCode);
                if (existsproductTuition)
                    throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);

                productEntity = await _productRepository.GetByTypeCode(productTuitionRequest.ProductTypeId.Value, productTuitionRequest.ProductCode) ??
                                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                await RetainProduct(productTuitionEntity, productEntity);
            }

            productTuitionEntity.ProductId = productEntity.Id;
            productTuitionEntity.CreatedBy = _email;
            productTuitionEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            productTuitionEntity = await _productTuitionRepository.CreateProductTuition(productTuitionEntity);

            CreateBills(productTuitionEntity);

            var module = JwtManager.GetModuleByToken(_httpContextAccessor);

            if (module == ProjectModules.Modules.ElementAt(1))
                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(0)); //delivery

            //Sempre que adicionado producttuition o contrato é atualizado
            //Se o contrato da nova locação desse produto ainda não foi assinado, apenas é editado o último contrato
            var LastContract = await _contractRepository.GetTheLastContract(productTuitionRequest.RentId.Value);

            if (LastContract.SignedAt != null)
            {
                //Se o contrato da locação desse produto já foi assinado, então cria um novo contrato
                var contractEntity = new ContractEntity()
                {
                    GuidId = Guid.NewGuid(),
                    RentId = productTuitionRequest.RentId.Value,
                    Version = LastContract.Version + 1,
                    UrlSignature = null,
                    SignedAt = null,
                    UpdatedAt = null,
                    UpdatedBy = null,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone),
                    CreatedBy = _email
                };

                await _contractRepository.CreateContract(contractEntity);
            }

            var productTuitionResponse = _mapper.Map<CreateProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task WithdrawProduct(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionEntity.ProductCode == null)
                throw new HttpRequestException("Não é possível retirar o produto sem produto", null, HttpStatusCode.BadRequest);

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1)) //Module Delivery
            {
                var deliveryOs = await _osRepository.GetByProductTuitionId(productTuitionEntity.Id, OsTypes.OsTypesEnum.ElementAt(0)); //delivery
                if (deliveryOs != null)
                {
                    if (deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(0) /*waiting*/ || deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(3) /*returned*/)
                    {
                        deliveryOs.Status = OsStatus.OsStatusEnum.ElementAt(4); //canceled
                        deliveryOs.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                        deliveryOs.UpdatedBy = _email;
                        await _osRepository.UpdateOs(deliveryOs);
                    }
                }

                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(1)); //withdraw
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4); //withdraw
                productTuitionEntity.IsEditable = false;
            }

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(0)) //Module Basic
            {
                var productEntity = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode);

                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4); //withdraw
                productTuitionEntity.IsEditable = false;
                await ReleaseProduct(productTuitionEntity, productEntity);

                FinishRentIfTheLast(productTuitionEntity);
            }

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);
        }

        public async Task CancelWithdrawProduct(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var osForDelete = new OsEntity();

            do
            {
                osForDelete = await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(1)); //withdraw

                if (osForDelete != null)
                {
                    osForDelete.Deleted = true;
                    osForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                    osForDelete.UpdatedBy = _email;

                    await _osRepository.UpdateOs(osForDelete);
                }

            } while (osForDelete != null);

            var deliveryOs = await _osRepository.GetByProductTuitionId(productTuitionEntity.Id, OsTypes.OsTypesEnum.ElementAt(0)); //delivery
            if (deliveryOs != null)
            {
                if (deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(4)) //canceled
                {
                    deliveryOs.Status = OsStatus.OsStatusEnum.ElementAt(0); //waiting
                    deliveryOs.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                    deliveryOs.UpdatedBy = _email;
                    await _osRepository.UpdateOs(deliveryOs);
                }
            }

            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2); //delivered
            productTuitionEntity.IsEditable = true;

            var rent = await _rentRepository.GetById(productTuitionEntity.RentId) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            if (rent.Status != RentStatus.RentStatusEnum.ElementAt(0)) //activated
            {
                rent.Status = RentStatus.RentStatusEnum.ElementAt(0); //activated
                rent.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                rent.UpdatedBy = _email;

                await _rentRepository.UpdateRent(rent);
            }

            productTuitionEntity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionEntity.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);
        }

        public async Task RenewProductTuition(int id, RenewProductTuitionRequest renewRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            renewRequest = TimeZoneConverter<RenewProductTuitionRequest>.ConvertToTimeZoneLocal(renewRequest, _timeZone);

            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionEntity.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4)) //withdraw
                throw new HttpRequestException("Não é possível renovar um produto em retirada", null, HttpStatusCode.BadRequest);

            productTuitionEntity.FinalDateTime = renewRequest.FinalDateTime.Value;
            productTuitionEntity.QuantityPeriod = renewRequest.QuantityPeriod.Value;
            productTuitionEntity.TimePeriod = renewRequest.TimePeriod;
            productTuitionEntity.Value = renewRequest.Value;
            productTuitionEntity.FirstDueDate = renewRequest.FirstDueDate;
            productTuitionEntity.FinalDateTime = renewRequest.FinalDateTime.Value;
            productTuitionEntity.IsEditable = false;
            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2); //delivered
            productTuitionEntity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionEntity.UpdatedBy = _email;

            CreateBills(productTuitionEntity);

            await _productTuitionRepository.UpdateProductTuition(productTuitionEntity);
        }

        public async Task<GetProductTuitionByIdResponse> GetProductTuitionById(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var productTuitionResponse = _mapper.Map<GetProductTuitionByIdResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<IEnumerable<GetAllProductTuitionByRentIdReponse>> GetAllProductTuitionByRentId(int rentId)
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllByRentId(rentId);

            var productTuitionEntityListLenght = productTuitionEntityList.ToList().Count;

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetAllProductTuitionByRentIdReponse>>(productTuitionEntityList);

            for (int i = 0; i < productTuitionEntityListLenght; i++)
            {
                productTuitionsResponseList.ElementAt(i).Rent = _mapper.Map<GetRentClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Address = _mapper.Map<CreateAddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.AddressEntity);
                productTuitionsResponseList.ElementAt(i).ProductType = _mapper.Map<CreateProductTypeResponse>(productTuitionEntityList.ElementAt(i).ProductTypeEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client = _mapper.Map<CreateClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client.Address = _mapper.Map<CreateAddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity.AddressEntity);
            }

            return productTuitionsResponseList;
        }

        public async Task<GetAllProductTuitionByProductIdResponse> GetAllProductTuitionByProductId(int productId)
        {
            var productEntity = await _productRepository.GetById(productId) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTuitionEntityList = await _productTuitionRepository.GetAllByProductTypeCode(productEntity.ProductTypeId, productEntity.Code);

            var productTuitionsResponse = new GetAllProductTuitionByProductIdResponse
            {
                ProductTuitionsRentResponse = _mapper.Map<List<ResumedRentDto>>(productTuitionEntityList),
                TotalBilledValue = 0
            };

            foreach (var productTuition in productTuitionsResponse.ProductTuitionsRentResponse)
            {
                var bills = (await _billRepository.GetByProductTuitionId(productTuition.Id)).ToList();

                foreach (var bill in bills)
                {
                    if (bill.Status == BillStatus.BillStatusEnum.ElementAt(1)) //payed
                    {
                        productTuitionsResponse.TotalBilledValue += bill.Value;
                        productTuition.BilledValue += bill.Value;
                    }
                }

                bills.Clear();
            }

            productTuitionsResponse.ProductTuitionsRentResponse.OrderByDescending(r => r.Id);

            return productTuitionsResponse;
        }

        //Refatorar
        public async Task<IEnumerable<GetAllProductTuitionToRemoveReponse>> GetAllProductTuitionToRemove()
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllToRemove();

            var productTuitionEntityListLenght = productTuitionEntityList.ToList().Count;

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetAllProductTuitionToRemoveReponse>>(productTuitionEntityList);

            for (int i = 0; i < productTuitionEntityListLenght; i++)
            {
                productTuitionsResponseList.ElementAt(i).Rent = _mapper.Map<GetRentClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Address = _mapper.Map<CreateAddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.AddressEntity);
                productTuitionsResponseList.ElementAt(i).ProductType = _mapper.Map<CreateProductTypeResponse>(productTuitionEntityList.ElementAt(i).ProductTypeEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client = _mapper.Map<CreateClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client.Address = _mapper.Map<CreateAddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity.AddressEntity);
            }

            return productTuitionsResponseList;
        }

        public async Task UpdateProductTuition(UpdateProductTuitionRequest productTuitionRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productTuitionRequest = TimeZoneConverter<UpdateProductTuitionRequest>.ConvertToTimeZoneLocal(productTuitionRequest, _timeZone);

            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionForUpdate.IsEditable == false)
                throw new HttpRequestException("Essa fatura de produto não é editável", null, HttpStatusCode.NotFound);

            //Não é possível alterar o período da locação (mês, dia, semana)
            if (productTuitionRequest.TimePeriod != productTuitionForUpdate.TimePeriod)
                throw new HttpRequestException("Não é possível alterar o período TimePeriod", null, HttpStatusCode.BadRequest);

            //Verifica se existe a nova locaçao que está sendo atualizada
            if (productTuitionRequest.RentId != productTuitionForUpdate.RentId)
            {
                var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId.Value);
                if (!existsRent)
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            //Verifica se existe o tipo de produto que está sendo atualizado
            if (productTuitionRequest.ProductTypeId != productTuitionForUpdate.ProductTypeId)
            {
                var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId.Value);
                if (!existsProductType)
                    throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

            int? newId = null;

            //Se estiver sendo atualizado o produto, algumas validações são realizadas
            if (productTuitionRequest.ProductCode != productTuitionForUpdate.ProductCode || productTuitionRequest.ProductTypeId != productTuitionForUpdate.ProductTypeId)
            {
                //Se o código novo que está sendo inserido não for nulo, é verificado se o produto com esse código existe e retém produto no estoque
                if (productTuitionRequest.ProductCode != null)
                {
                    var productEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionRequest.ProductCode) ??
                        throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    await RetainProduct(_mapper.Map<ProductTuitionEntity>(productTuitionRequest), productEntity);
                    newId = productEntity.Id;
                }

                //Se o código antigo do produto não for nulo, produto é liberado no estoque
                if (productTuitionForUpdate.ProductCode != null)
                {
                    var oldProductEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionForUpdate.ProductCode) ??
                        throw new HttpRequestException("Não foi possível encontrar um produto", null, HttpStatusCode.InternalServerError);
                    await ReleaseProduct(productTuitionForUpdate, oldProductEntity);
                }
            }

            //Verifica se o produto é o mesmo e está sendo alterado a quantidade de peças "parts"
            if (productTuitionRequest.ProductCode == productTuitionForUpdate.ProductCode && productTuitionRequest.ProductTypeId == productTuitionForUpdate.ProductTypeId && productTuitionRequest.Parts != productTuitionForUpdate.Parts)
            {
                //Se realmente existir o produto, então o mesmo é liberado do estoque e retido novamente com a quantidade de peças modificadas
                if (productTuitionRequest.ProductCode != null)
                {
                    var productEntityForParts = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionRequest.ProductCode) ??
                        throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    var productReleased = await ReleaseProduct(productTuitionForUpdate, productEntityForParts);
                    await RetainProduct(_mapper.Map<ProductTuitionEntity>(productTuitionRequest), productReleased);
                }
            }

            //Mapeia o request para a entidade e atribui o id do producttuition a ser atualizado, parecido como se puxasse do banco de dados para atualizar
            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);
            productTuitionEntity.Id = id;

            //Valida se é necessário recalcular as parcelas do produto conforme informações novas, qualquer condição a seguir já atualiza as parcelas por completo            
            //Valida se está sendo alterado: o valor do produto ou a data do primeiro vencimento ou a data de início da locação ou a data do período de locação
            if (productTuitionRequest.Value != productTuitionForUpdate.Value ||
                productTuitionRequest.FirstDueDate != productTuitionForUpdate.FirstDueDate ||
                productTuitionRequest.InitialDateTime != productTuitionForUpdate.InitialDateTime ||
                (productTuitionRequest.FinalDateTime != productTuitionForUpdate.FinalDateTime && productTuitionRequest.InitialDateTime == productTuitionForUpdate.InitialDateTime))
            {
                UpdateBills(productTuitionEntity);
            }

            //Sempre que alterado o producttuition de uma rent para outra os contratos são atualizados
            if (productTuitionRequest.RentId != productTuitionForUpdate.RentId)
            {
                //Se o contrato da nova locação desse produto já foi assinado, então cria um novo contrato
                var productRequestContract = await _contractRepository.GetTheLastContract(productTuitionRequest.RentId.Value);

                if (productRequestContract.SignedAt != null)
                {
                    var contractEntity = new ContractEntity()
                    {
                        GuidId = Guid.NewGuid(),
                        RentId = productTuitionRequest.RentId.Value,
                        Version = productRequestContract.Version + 1,
                        UrlSignature = null,
                        SignedAt = null,
                        UpdatedAt = null,
                        UpdatedBy = null,
                        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone),
                        CreatedBy = _email
                    };

                    await _contractRepository.CreateContract(contractEntity);
                }

                //Se o contrato da antiga locação desse produto já foi assinado, então cria um novo contrato
                var productForUpdateContract = await _contractRepository.GetTheLastContract(productTuitionForUpdate.RentId);

                if (productForUpdateContract.SignedAt != null)
                {
                    var contractEntity = new ContractEntity()
                    {
                        GuidId = Guid.NewGuid(),
                        RentId = productTuitionForUpdate.RentId,
                        Version = await _contractRepository.GetTheLastVersion(productTuitionForUpdate.RentId) + 1,
                        UrlSignature = null,
                        SignedAt = null,
                        UpdatedAt = null,
                        UpdatedBy = null,
                        CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone),
                        CreatedBy = _email
                    };

                    await _contractRepository.CreateContract(contractEntity);
                }
            }

            productTuitionForUpdate.ProductId = newId;
            productTuitionForUpdate.RentId = productTuitionRequest.RentId.Value;
            productTuitionForUpdate.ProductTypeId = productTuitionRequest.ProductTypeId.Value;
            productTuitionForUpdate.ProductCode = productTuitionRequest.ProductCode;
            productTuitionForUpdate.Value = productTuitionRequest.Value;
            productTuitionForUpdate.InitialDateTime = productTuitionRequest.InitialDateTime.Value;
            productTuitionForUpdate.FinalDateTime = productTuitionRequest.FinalDateTime.Value;
            productTuitionForUpdate.Parts = productTuitionRequest.Parts.Value;
            productTuitionForUpdate.Status = productTuitionRequest.Status;
            productTuitionForUpdate.FirstDueDate = productTuitionRequest.FirstDueDate;
            productTuitionForUpdate.QuantityPeriod = productTuitionRequest.QuantityPeriod.Value;
            productTuitionForUpdate.TimePeriod = productTuitionRequest.TimePeriod;
            productTuitionForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionForUpdate.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate);
        }

        public async Task UpdateProductCode(string productCode, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            int? newId = null;

            if (productCode != productTuitionForUpdate.ProductCode)
            {
                //retém no estoque o produto do código que está sendo inserido
                if (productCode != null)
                {
                    var productEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productCode) ??
                                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    await RetainProduct(productTuitionForUpdate, productEntity);
                    newId = productEntity.Id;
                }

                //libera no estoque o produto que existia anteriormente nesse producttuition e estava retido
                if (productTuitionForUpdate.ProductCode != null)
                {
                    var oldProductEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionForUpdate.ProductCode) ??
                                throw new HttpRequestException("Não foi possível encontrar um produto", null, HttpStatusCode.InternalServerError);
                    await ReleaseProduct(productTuitionForUpdate, oldProductEntity);
                }
            }

            productTuitionForUpdate.ProductId = newId;
            productTuitionForUpdate.ProductCode = productCode;
            productTuitionForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionForUpdate.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForUpdate.Status = status;
            productTuitionForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionForUpdate.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var productTuitionForDelete = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionForDelete.ProductCode != null)
            {
                var oldProductEntity = await _productRepository.GetByTypeCode(productTuitionForDelete.ProductTypeId, productTuitionForDelete.ProductCode) ??
                    throw new HttpRequestException("Não foi possível encontrar o produto antigo", null, HttpStatusCode.InternalServerError);
                await ReleaseProduct(productTuitionForDelete, oldProductEntity);
            }

            var bills = (await _billRepository.GetByProductTuitionId(id)).ToList();

            foreach (var bill in bills)
            {
                await _billService.DeleteById(bill.Id);
            }

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1)) //delivery
            {
                var deliveryOs = (await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(0))); //delivery
                var withdrawOs = (await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(1))); //withdraw

                if (deliveryOs != null)
                {
                    deliveryOs.Deleted = true;
                    deliveryOs.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                    deliveryOs.UpdatedBy = _email;
                    await _osRepository.UpdateOs(deliveryOs);
                }

                if (withdrawOs != null)
                {
                    withdrawOs.Deleted = true;
                    withdrawOs.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                    withdrawOs.UpdatedBy = _email;
                    await _osRepository.UpdateOs(withdrawOs);
                }
            }

            //Se o contrato da locação desse produto já foi assinado, então cria um novo contrato
            var lastContract = await _contractRepository.GetTheLastContract(productTuitionForDelete.RentId);

            if (lastContract.SignedAt != null)
            {
                var contractEntity = new ContractEntity()
                {
                    GuidId = Guid.NewGuid(),
                    RentId = productTuitionForDelete.RentId,
                    Version = lastContract.Version + 1,
                    UrlSignature = null,
                    SignedAt = null,
                    UpdatedAt = null,
                    UpdatedBy = null,
                    CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone),
                    CreatedBy = _email
                };

                await _contractRepository.CreateContract(contractEntity);
            }

            productTuitionForDelete.Deleted = true;
            productTuitionForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTuitionForDelete.UpdatedBy = _email;

            await _productTuitionRepository.UpdateProductTuition(productTuitionForDelete);
        }

        public IEnumerable<BillEntity> CreateBills(ProductTuitionEntity productTuition)
        {
            var billsQuantity = 0;
            if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(0) /*day*/ || productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(1) /*week*/)
                billsQuantity = 1;
            else if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(2)) //month
                billsQuantity = productTuition.QuantityPeriod;

            var createdBills = new List<BillEntity>();

            for (int i = 0, y = 1; i < billsQuantity; i++, y++)
            {
                var bill = new BillEntity();

                bill.RentId = productTuition.RentId;
                bill.Order = y;
                bill.ProductTuitionId = productTuition.Id;
                bill.Value = productTuition.Value;
                bill.DueDate = productTuition.FirstDueDate.Value.AddMonths(i);
                bill.Status = BillStatus.BillStatusEnum.ElementAt(0); //open
                bill.PaymentMode = null;
                bill.CreatedBy = _email;

                createdBills.Add(_billRepository.CreateBill(bill).Result);
            }

            return createdBills;
        }

        public void UpdateBills(ProductTuitionEntity productTuition)
        {
            var oldBills = (_billRepository.GetByProductTuitionId(productTuition.Id)).Result.OrderBy(b => b.Order).ToList();
            var payedOldBills = new List<BillEntity>();

            foreach (var bill in oldBills)
            {
                if (bill.Status != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && bill.NfIdFireBase == null)
                    _billRepository.DeleteBill(bill);
                else payedOldBills.Add(bill);
            }

            var newBills = CreateBills(productTuition).ToList();

            foreach (var bill in payedOldBills)
            {
                var newBillForDelete = newBills.FirstOrDefault(b => b.Order == bill.Order);

                if (newBillForDelete != null)
                    _billRepository.DeleteBill(newBillForDelete);
            }
        }

        public async void FinishRentIfTheLast(ProductTuitionEntity productTuitionEntity)
        {
            var productTuitionsRentList = (await _productTuitionRepository.GetAllByRentId(productTuitionEntity.RentId)).ToList();

            productTuitionsRentList.Remove(productTuitionEntity);

            var isNotTheLastProductTuitionOfRent = productTuitionsRentList.Exists(p => p.Status != ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5)); //returned

            if (isNotTheLastProductTuitionOfRent == false)
            {
                var rent = await _rentRepository.GetById(productTuitionEntity.RentId);
                rent.Status = RentStatus.RentStatusEnum.ElementAt(1); //finished
                rent.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                rent.UpdatedBy = _email;

                await _rentRepository.UpdateRent(rent);
            }
        }

        public void CreateOs(ProductTuitionEntity productTuition, string type)
        {
            var os = new OsEntity();

            os.ProductTuitionId = productTuition.Id;
            os.Type = type;
            os.Status = OsStatus.OsStatusEnum.ElementAt(0); //waiting
            os.CreatedBy = _email;

            _osRepository.CreateOs(os);
        }

        public async Task<ProductEntity> RetainProduct(ProductTuitionEntity productTuition, ProductEntity productEntity)
        {
            if (productEntity.Status == ProductStatus.ProductStatusEnum.ElementAt(1)) //rented
                throw new HttpRequestException("Esse produto está em outra locação", null, HttpStatusCode.BadRequest);
            if (productEntity.Status == ProductStatus.ProductStatusEnum.ElementAt(2)) //maintenance
                throw new HttpRequestException("Esse produto está em manutenção", null, HttpStatusCode.BadRequest);
            if (productEntity.Parts - productEntity.RentedParts < productTuition.Parts)
                throw new HttpRequestException("Esse produto não possui essa quantidade de partes disponíveis", null, HttpStatusCode.BadRequest);

            productEntity.RentedParts += productTuition.Parts;
            productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(0); //free
            if (!productEntity.ProductTypeEntity.IsManyParts) productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(1); //rented

            if (await _productRepository.UpdateProduct(productEntity) == 0)
                throw new HttpRequestException("Não foi possível atualizar o produto novo", null, HttpStatusCode.InternalServerError);

            return productEntity;
        }

        public async Task<ProductEntity> ReleaseProduct(ProductTuitionEntity productTuition, ProductEntity productEntity)
        {
            productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(0); //free
            productEntity.RentedParts -= productTuition.Parts;
            if (await _productRepository.UpdateProduct(productEntity) == 0)
                throw new HttpRequestException("Não foi possível atualizar um produto para disponível", null, HttpStatusCode.InternalServerError);

            return productEntity;
        }
    }
}