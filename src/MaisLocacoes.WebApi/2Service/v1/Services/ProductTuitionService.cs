using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class ProductTuitionService : IProductTuitionService
    {
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IBillRepository _billRepository;
        private readonly IOsRepository _osRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IRentRepository rentRepository,
            IBillRepository billRepository,
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
            _osRepository = osRepository;
            _productRepository = productRepository;
            _productService = productService;
            _productTypeRepository = productTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ProductTuitionResponse> CreateProductTuition(ProductTuitionRequest productTuitionRequest)
        {
            var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId);
            if (!existsRent)
            {
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId);
            if (!existsProductType)
            {
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

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

            productTuitionEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productTuitionEntity = await _productTuitionRepository.CreateProductTuition(productTuitionEntity);

            CreateBills(productTuitionEntity);

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1))
                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(0));

            var productTuitionResponse = _mapper.Map<ProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<bool> WithdrawProduct(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionEntity.ProductCode == null)
                throw new HttpRequestException("Não é possível cancelar uma fatura de produto sem produto", null, HttpStatusCode.BadRequest);

            var productEntity = await _productRepository.GetByTypeCode(productTuitionEntity.ProductTypeId, productTuitionEntity.ProductCode) ??
                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(1))
            {
                CreateOs(productTuitionEntity, OsTypes.OsTypesEnum.ElementAt(1));
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4);
                await ReleaseProduct(productTuitionEntity, productEntity);
            }

            if (JwtManager.GetModuleByToken(_httpContextAccessor) == ProjectModules.Modules.ElementAt(0))
            {
                productTuitionEntity.Status = ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(4);
                await ReleaseProduct(productTuitionEntity, productEntity);
            }

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionEntity) > 0) return true;
            else return false;
        }

        public async Task<GetProductTuitionRentResponse> GetById(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var productTuitionResponse = _mapper.Map<GetProductTuitionRentResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllByRentId(int rentId)
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllByRentId(rentId);

            var productTuitionEntityListLenght = productTuitionEntityList.ToList().Count;

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetProductTuitionRentProductTypeClientReponse>>(productTuitionEntityList);

            for (int i = 0; i < productTuitionEntityListLenght; i++)
            {
                productTuitionsResponseList.ElementAt(i).Rent = _mapper.Map<GetRentClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Address = _mapper.Map<AddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.AddressEntity);
                productTuitionsResponseList.ElementAt(i).ProductType = _mapper.Map<ProductTypeResponse>(productTuitionEntityList.ElementAt(i).ProductTypeEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client = _mapper.Map<ClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client.Address = _mapper.Map<AddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity.AddressEntity);
            }

            return productTuitionsResponseList;
        }

        public async Task<IEnumerable<GetProductTuitionRentResponse>> GetAllByProductId(int productId)
        {
            var productEntity = await _productRepository.GetById(productId) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTuitionEntityList = await _productTuitionRepository.GetAllByProductTypeCode(productEntity.ProductTypeId, productEntity.Code);

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetProductTuitionRentResponse>>(productTuitionEntityList);

            return productTuitionsResponseList;
        }

        public async Task<IEnumerable<GetProductTuitionRentProductTypeClientReponse>> GetAllToRemove()
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllToRemove();

            var productTuitionEntityListLenght = productTuitionEntityList.ToList().Count;

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetProductTuitionRentProductTypeClientReponse>>(productTuitionEntityList);

            for (int i = 0; i < productTuitionEntityListLenght; i++)
            {
                productTuitionsResponseList.ElementAt(i).Rent = _mapper.Map<GetRentClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Address = _mapper.Map<AddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.AddressEntity);
                productTuitionsResponseList.ElementAt(i).ProductType = _mapper.Map<ProductTypeResponse>(productTuitionEntityList.ElementAt(i).ProductTypeEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client = _mapper.Map<ClientResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Client.Address = _mapper.Map<AddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.ClientEntity.AddressEntity);
            }

            return productTuitionsResponseList;
        }

        public async Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

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
            productTuitionForUpdate.UpdatedAt = System.DateTime.Now;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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
            productTuitionForUpdate.UpdatedAt = System.DateTime.Now;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForUpdate.Status = status;
            productTuitionForUpdate.UpdatedAt = System.DateTime.Now;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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

            productTuitionForDelete.Deleted = true;
            productTuitionForDelete.UpdatedAt = System.DateTime.Now;
            productTuitionForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForDelete) > 0) return true;
            else return false;
        }

        public void CreateBills(ProductTuitionEntity productTuition)
        {
            var billsQuantity = 0;
            if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(0) || productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(1))
                billsQuantity = 1;
            else if (productTuition.TimePeriod == ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(2))
                billsQuantity = productTuition.QuantityPeriod;

            for (int i = 0; i < billsQuantity; i++)
            {
                var bill = new BillEntity();

                bill.RentId = productTuition.RentId;
                bill.ProductTuitionId = productTuition.Id;
                bill.Value = productTuition.Value;
                bill.DueDate = productTuition.FirstDueDate.Value.AddMonths(i);
                bill.Status = BillStatus.BillStatusEnum.ElementAt(0);
                bill.PaymentMode = null;
                bill.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                _billRepository.CreateBill(bill);
            }
        }

        public void CreateOs(ProductTuitionEntity productTuition, string type)
        {
            var os = new OsEntity();

            os.ProductTuitionId = productTuition.Id;
            os.Type = type;
            os.Status = OsStatus.OsStatusEnum.ElementAt(0);
            os.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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