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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IRentRepository rentRepository,
            IBillRepository billRepository,
            IOsRepository osRepository,
            IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTuitionRepository = productTuitionRepository;
            _rentRepository = rentRepository;
            _billRepository = billRepository;
            _osRepository = osRepository;
            _productRepository = productRepository;
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

            if (!string.IsNullOrEmpty(productTuitionRequest.ProductCode))
            {
                var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId, productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode);
                if (existsproductTuition)
                {
                    throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);
                }
                var product = await _productRepository.GetByTypeCode(productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode) ??
                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(1))
                    throw new HttpRequestException("Produto já alugado", null, HttpStatusCode.BadRequest);
                if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(2))
                    throw new HttpRequestException("Produto em manutenção", null, HttpStatusCode.BadRequest);
            }

            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);

            productTuitionEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productTuitionEntity = await _productTuitionRepository.CreateProductTuition(productTuitionEntity);

            CreateBills(productTuitionEntity);
            CreateOs(productTuitionEntity);

            var productTuitionResponse = _mapper.Map<ProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<GetProductTuitionRentResponse> GetById(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var productTuitionResponse = _mapper.Map<GetProductTuitionRentResponse>(productTuitionEntity);
            var rentResponse = _mapper.Map<RentResponse>(productTuitionEntity.RentEntity);
            var rentAddressResponse = _mapper.Map<AddressResponse>(productTuitionEntity.RentEntity.AddressEntity);

            productTuitionResponse.Rent = rentResponse;
            productTuitionResponse.Rent.Address = rentAddressResponse;

            return productTuitionResponse;
        }

        public async Task<IEnumerable<ProductTuitionResponse>> GetAllByRentId(int rentId)
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllByRentId(rentId);

            var productTuitionsResponseList = _mapper.Map<IEnumerable<ProductTuitionResponse>>(productTuitionEntityList);

            return productTuitionsResponseList;
        }

        public async Task<IEnumerable<GetProductTuitionRentResponse>> GetAllByProductId(int productId)
        {
            var productEntity = await _productRepository.GetById(productId) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTuitionEntityList = await _productTuitionRepository.GetAllByProductTypeCode(productEntity.ProductTypeId, productEntity.Code);

            var productsEntityListLenght = productTuitionEntityList.ToList().Count;

            var productTuitionsResponseList = _mapper.Map<IEnumerable<GetProductTuitionRentResponse>>(productTuitionEntityList);

            for (int i = 0; i < productsEntityListLenght; i++)
            {
                productTuitionsResponseList.ElementAt(i).Rent = _mapper.Map<RentResponse>(productTuitionEntityList.ElementAt(i).RentEntity);
                productTuitionsResponseList.ElementAt(i).Rent.Address = _mapper.Map<AddressResponse>(productTuitionEntityList.ElementAt(i).RentEntity.AddressEntity);
            }

            return productTuitionsResponseList;
        }

        public async Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionRequest.RentId != productTuitionForUpdate.RentId || productTuitionRequest.ProductTypeId != productTuitionForUpdate.ProductTypeId || productTuitionRequest.ProductCode != productTuitionForUpdate.ProductCode)
            {
                if (productTuitionRequest.RentId != productTuitionForUpdate.RentId)
                {
                    var existsRent = await _rentRepository.RentExists(productTuitionRequest.RentId);
                    if (!existsRent)
                    {
                        throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
                    }
                }

                if (!string.IsNullOrEmpty(productTuitionRequest.ProductCode))
                {
                    var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId, productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode);
                    if (existsproductTuition)
                    {
                        throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);
                    }

                    var product = await _productRepository.GetByTypeCode(productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode) ??
                    throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
                    if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(1))
                        throw new HttpRequestException("Esse produto está em outra locação", null, HttpStatusCode.BadRequest);
                    if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(2))
                        throw new HttpRequestException("Esse produto está em manutenção", null, HttpStatusCode.BadRequest);
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
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateProductCode(string productCode, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var product = await _productRepository.GetByTypeCode(productTuitionForUpdate.ProductTypeId, productCode) ??
                throw new HttpRequestException("Esse produto não existe", null, HttpStatusCode.BadRequest);
            if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(1))
                throw new HttpRequestException("Esse produto está em outra locação", null, HttpStatusCode.BadRequest);
            if (product.Status == ProductStatus.ProductStatusEnum.ElementAt(2))
                throw new HttpRequestException("Esse produto está em manutenção", null, HttpStatusCode.BadRequest);

            productTuitionForUpdate.ProductCode = productCode;
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForUpdate.Status = status;
            productTuitionForUpdate.UpdatedAt = System.DateTime.UtcNow;
            productTuitionForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productTuitionForDelete = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForDelete.Deleted = true;
            productTuitionForDelete.UpdatedAt = System.DateTime.UtcNow;
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

                bill.ProductTuitionId = productTuition.Id;
                bill.Value = productTuition.Value;
                bill.DueDate = productTuition.FirstDueDate.Value.AddMonths(i);
                bill.Status = BillStatus.BillStatusEnum.ElementAt(0);
                bill.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                _billRepository.CreateBill(bill);
            }
        }

        public void CreateOs(ProductTuitionEntity productTuition)
        {
            var os = new OsEntity();

            os.ProductTuitionId = productTuition.Id;
            os.Status = OsStatus.OsStatusEnum.ElementAt(0);
            os.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            _osRepository.CreateOs(os);
        }
    }
}