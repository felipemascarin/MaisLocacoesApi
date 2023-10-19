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
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Get.GetAllProductTuitionByProductIdResponse;

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
        private readonly IProductService _productService;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeSpan _timeZone;
        private readonly string _email;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IRentRepository rentRepository,
            IBillRepository billRepository,
            IBillService billService,
            IOsRepository osRepository,
            IProductRepository productRepository,
            IProductService productService,
            IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTuitionRepository = productTuitionRepository;
            _rentRepository = rentRepository;
            _billRepository = billRepository;
            _billService = billService;
            _osRepository = osRepository;
            _productRepository = productRepository;
            _productService = productService;
            _productTypeRepository = productTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductTuitionResponse> CreateProductTuition(CreateProductTuitionRequest productTuitionRequest)
        {
            var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId);
            if (!existsRent)
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);

            var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId);
            if (!existsProductType)
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);

            if (!string.IsNullOrEmpty(productTuitionRequest.ProductCode))
            {
                var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId, productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode);
                if (existsproductTuition)
                    throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);

                var productEntity = await _productRepository.GetByTypeCode(productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode) ??
                                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                await RetainProduct(productTuitionEntity, productEntity);
            }

            productTuitionEntity.CreatedBy = _email;
            productTuitionEntity.CreatedAt = System.DateTime.UtcNow + _timeZone;

            productTuitionEntity = await _productTuitionRepository.CreateProductTuition(productTuitionEntity);

            CreateBills(productTuitionEntity);

            var module = JwtManager.GetModuleByToken(_httpContextAccessor);

            if (module == ProjectModules.Modules.ElementAt(1))
                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(0));

            var productTuitionResponse = _mapper.Map<CreateProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<bool> WithdrawProduct(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionEntity.ProductCode == null)
                throw new HttpRequestException("Não é possível retirar o produto sem produto", null, HttpStatusCode.BadRequest);

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1)) //Module Delivery
            {
                var deliveryOs = await _osRepository.GetByProductTuitionId(productTuitionEntity.Id, OsTypes.OsTypesEnum.ElementAt(0));
                if (deliveryOs != null)
                {
                    if (deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(0) || deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(3))
                    {
                        deliveryOs.Status = OsStatus.OsStatusEnum.ElementAt(4);
                        deliveryOs.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                        deliveryOs.UpdatedBy = _email;
                        await _osRepository.UpdateOs(deliveryOs);
                    }
                }

                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(1));
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4);
                productTuitionEntity.IsEditable = false;
            }

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(0)) //Module Basic
            {
                var productEntity = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode);

                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4);
                productTuitionEntity.IsEditable = false;
                await ReleaseProduct(productTuitionEntity, productEntity);

                FinishRentIfTheLast(productTuitionEntity);
            }

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionEntity) > 0) return true;
            else return false;
        }

        public async Task<bool> CancelWithdrawProduct(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var osForDelete = new OsEntity();

            do
            {
                osForDelete = await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(1));

                if (osForDelete != null)
                {
                    osForDelete.Deleted = true;
                    osForDelete.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                    osForDelete.UpdatedBy = _email;

                    await _osRepository.UpdateOs(osForDelete);
                }

            } while (osForDelete != null);

            var deliveryOs = await _osRepository.GetByProductTuitionId(productTuitionEntity.Id, OsTypes.OsTypesEnum.ElementAt(0));
            if (deliveryOs != null)
            {
                if (deliveryOs.Status == OsStatus.OsStatusEnum.ElementAt(4))
                {
                    deliveryOs.Status = OsStatus.OsStatusEnum.ElementAt(0);
                    deliveryOs.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                    deliveryOs.UpdatedBy = _email;
                    await _osRepository.UpdateOs(deliveryOs);
                }
            }

            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2);
            productTuitionEntity.IsEditable = true;

            var rent = await _rentRepository.GetById(productTuitionEntity.RentId) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            if (rent.Status != RentStatus.RentStatusEnum.ElementAt(0))
            {
                rent.Status = RentStatus.RentStatusEnum.ElementAt(0);
                rent.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                rent.UpdatedBy = _email;

                await _rentRepository.UpdateRent(rent);
            }

            productTuitionEntity.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionEntity.UpdatedBy = _email;

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionEntity) > 0) return true;
            else return false;
        }

        public async Task<bool> RenewProductTuition(int id, RenewProductTuitionRequest renewRequest)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionEntity.Status == ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4))
                throw new HttpRequestException("Não é possível renovar um produto em retirada", null, HttpStatusCode.BadRequest);

            productTuitionEntity.FinalDateTime = renewRequest.FinalDateTime;
            productTuitionEntity.QuantityPeriod = renewRequest.QuantityPeriod;
            productTuitionEntity.TimePeriod = renewRequest.TimePeriod;
            productTuitionEntity.Value = renewRequest.Value;
            productTuitionEntity.FirstDueDate = renewRequest.FirstDueDate;
            productTuitionEntity.FinalDateTime = renewRequest.FinalDateTime;
            productTuitionEntity.IsEditable = false;
            productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(2);
            productTuitionEntity.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionEntity.UpdatedBy = _email;

            CreateBills(productTuitionEntity);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionEntity) > 0) return true;
            else return false;
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
                    if (bill.Status == BillStatus.BillStatusEnum.ElementAt(1))
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

        public async Task<bool> UpdateProductTuition(UpdateProductTuitionRequest productTuitionRequest, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionForUpdate.IsEditable == false)
                throw new HttpRequestException("Essa fatura de produto não é editável", null, HttpStatusCode.NotFound);

            if (productTuitionRequest.TimePeriod != productTuitionForUpdate.TimePeriod)
                throw new HttpRequestException("Não é possível alterar o período TimePeriod", null, HttpStatusCode.BadRequest);

            if (productTuitionRequest.RentId != productTuitionForUpdate.RentId)
            {
                var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
                }
            }

            if (productTuitionRequest.ProductTypeId != productTuitionForUpdate.ProductTypeId)
            {
                var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId);
                if (!existsProductType)
                    throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

            if (productTuitionRequest.ProductCode != productTuitionForUpdate.ProductCode)
            {
                if (productTuitionRequest.ProductCode != null)
                {
                    var productEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionRequest.ProductCode) ??
                        throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    await RetainProduct(_mapper.Map<ProductTuitionEntity>(productTuitionRequest), productEntity);
                }

                if (productTuitionForUpdate.ProductCode != null)
                {
                    var oldProductEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionForUpdate.ProductCode) ??
                        throw new HttpRequestException("Não foi possível encontrar um produto", null, HttpStatusCode.InternalServerError);
                    await ReleaseProduct(productTuitionForUpdate, oldProductEntity);
                }
            }

            if (productTuitionRequest.ProductCode == productTuitionForUpdate.ProductCode && productTuitionRequest.Parts != productTuitionForUpdate.Parts)
            {
                if (productTuitionRequest.ProductCode != null)
                {
                    var productEntityForParts = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionRequest.ProductCode) ??
                        throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    var productReleased = await ReleaseProduct(productTuitionForUpdate, productEntityForParts);
                    await RetainProduct(_mapper.Map<ProductTuitionEntity>(productTuitionRequest), productReleased);
                }
            }

            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);
            productTuitionEntity.Id = id;

            var billsUpdated = false;

            if (productTuitionRequest.Value != productTuitionForUpdate.Value && billsUpdated == false)
            {
                billsUpdated = true;
                UpdateBills(productTuitionEntity);
            }

            if (productTuitionRequest.FirstDueDate != productTuitionForUpdate.FirstDueDate && billsUpdated == false)
            {
                billsUpdated = true;
                UpdateBills(productTuitionEntity);
            }

            if (productTuitionRequest.InitialDateTime != productTuitionForUpdate.InitialDateTime && billsUpdated == false)
            {
                billsUpdated = true;
                UpdateBills(productTuitionEntity);
            }

            if (productTuitionRequest.FinalDateTime != productTuitionForUpdate.FinalDateTime && productTuitionRequest.InitialDateTime == productTuitionForUpdate.InitialDateTime && billsUpdated == false)
            {
                billsUpdated = true;
                UpdateBills(productTuitionEntity);
            }

            productTuitionForUpdate.RentId = productTuitionRequest.RentId;
            productTuitionForUpdate.ProductTypeId = productTuitionRequest.ProductTypeId;
            productTuitionForUpdate.ProductCode = productTuitionRequest.ProductCode;
            productTuitionForUpdate.Value = productTuitionRequest.Value;
            productTuitionForUpdate.InitialDateTime = productTuitionRequest.InitialDateTime;
            productTuitionForUpdate.FinalDateTime = productTuitionRequest.FinalDateTime;
            productTuitionForUpdate.Parts = productTuitionRequest.Parts;
            productTuitionForUpdate.Status = productTuitionRequest.Status;
            productTuitionForUpdate.FirstDueDate = productTuitionRequest.FirstDueDate;
            productTuitionForUpdate.QuantityPeriod = productTuitionRequest.QuantityPeriod;
            productTuitionForUpdate.TimePeriod = productTuitionRequest.TimePeriod;
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionForUpdate.UpdatedBy = _email;

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateProductCode(string productCode, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (productCode != productTuitionForUpdate.ProductCode)
            {
                if (productCode != null)
                {
                    var productEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productCode) ??
                                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    await RetainProduct(productTuitionForUpdate, productEntity);
                }

                if (productTuitionForUpdate.ProductCode != null)
                {
                    var oldProductEntity = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productTuitionForUpdate.ProductCode) ??
                                throw new HttpRequestException("Não foi possível encontrar um produto", null, HttpStatusCode.InternalServerError);
                    await ReleaseProduct(productTuitionForUpdate, oldProductEntity);
                }
            }

            productTuitionForUpdate.ProductCode = productCode;
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionForUpdate.UpdatedBy = _email;

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForUpdate.Status = status;
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionForUpdate.UpdatedBy = _email;

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
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
                _ = await _billService.DeleteById(bill.Id);
            }

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1)) //Module Delivery
            {
                var deliveryOs = (await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(0)));
                var withdrawOs = (await _osRepository.GetByProductTuitionId(id, OsTypes.OsTypesEnum.ElementAt(1)));

                if (deliveryOs != null)
                {
                    deliveryOs.Deleted = true;
                    deliveryOs.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                    deliveryOs.UpdatedBy = _email;
                    _ = await _osRepository.UpdateOs(deliveryOs);
                }

                if (withdrawOs != null)
                {
                    withdrawOs.Deleted = true;
                    withdrawOs.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                    withdrawOs.UpdatedBy = _email;
                    _ = await _osRepository.UpdateOs(withdrawOs);
                }
            }

            productTuitionForDelete.Deleted = true;
            productTuitionForDelete.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionForDelete.UpdatedBy = _email;

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForDelete) > 0) return true;
            else return false;
        }

        public IEnumerable<BillEntity> CreateBills(ProductTuitionEntity productTuition)
        {
            var billsQuantity = 0;
            if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(0) || productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(1))
                billsQuantity = 1;
            else if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(2))
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
                bill.Status = BillStatus.BillStatusEnum.ElementAt(0);
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
                if (bill.Status != BillStatus.BillStatusEnum.ElementAt(1) && bill.NfIdFireBase == null)
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

            var isNotTheLastProductTuitionOfRent = productTuitionsRentList.Exists(p => p.Status != ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(5));

            if (isNotTheLastProductTuitionOfRent == false)
            {
                var rent = await _rentRepository.GetById(productTuitionEntity.RentId);
                rent.Status = RentStatus.RentStatusEnum.ElementAt(1);
                rent.UpdatedAt = System.DateTime.UtcNow + _timeZone;
                rent.UpdatedBy = _email;

                await _rentRepository.UpdateRent(rent);
            }
        }

        public void CreateOs(ProductTuitionEntity productTuition, string type)
        {
            var os = new OsEntity();

            os.ProductTuitionId = productTuition.Id;
            os.Type = type;
            os.Status = OsStatus.OsStatusEnum.ElementAt(0);
            os.CreatedBy = _email;

            _osRepository.CreateOs(os);
        }

        public async Task<ProductEntity> RetainProduct(ProductTuitionEntity productTuition, ProductEntity productEntity)
        {
            if (productEntity.Status == ProductStatus.ProductStatusEnum.ElementAt(1))
                throw new HttpRequestException("Esse produto está em outra locação", null, HttpStatusCode.BadRequest);
            if (productEntity.Status == ProductStatus.ProductStatusEnum.ElementAt(2))
                throw new HttpRequestException("Esse produto está em manutenção", null, HttpStatusCode.BadRequest);
            if (productEntity.Parts - productEntity.RentedParts < productTuition.Parts)
                throw new HttpRequestException("Esse produto não possui essa quantidade de partes disponíveis", null, HttpStatusCode.BadRequest);

            productEntity.RentedParts += productTuition.Parts;
            productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(0);
            if (!productEntity.ProductTypeEntity.IsManyParts) productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(1);

            if (await _productRepository.UpdateProduct(productEntity) == 0)
                throw new HttpRequestException("Não foi possível atualizar o produto novo", null, HttpStatusCode.InternalServerError);

            return productEntity;
        }

        public async Task<ProductEntity> ReleaseProduct(ProductTuitionEntity productTuition, ProductEntity productEntity)
        {
            productEntity.Status = ProductStatus.ProductStatusEnum.ElementAt(0);
            productEntity.RentedParts -= productTuition.Parts;
            if (await _productRepository.UpdateProduct(productEntity) == 0)
                throw new HttpRequestException("Não foi possível atualizar um produto para disponível", null, HttpStatusCode.InternalServerError);

            return productEntity;
        }
    }
}