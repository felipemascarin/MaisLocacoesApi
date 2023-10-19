using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace MaisLocacoes.WebApi._2_Service.v1.Services
{
    public class ProductTuitionValueService : IProductTuitionValueService
    {
        private readonly IProductTuitionValueRepository _productTuitionValueRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeSpan _timeZone;
        private readonly string _email;

        public ProductTuitionValueService(IProductTuitionValueRepository productTuitionValueRepository,
            IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTuitionValueRepository = productTuitionValueRepository;
            _productTypeRepository = productTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TimeSpan.FromHours(int.Parse(JwtManager.GetTimeZoneByToken(_httpContextAccessor)));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductTuitionValueResponse> CreateProductTuitionValue(CreateProductTuitionValueRequest productTuitionValueRequest)
        {
            var existsProductType = await _productTypeRepository.ProductTypeExists(productTuitionValueRequest.ProductTypeId);
            if (!existsProductType)
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            var existsProductTuitionValue = await _productTuitionValueRepository.ProductTuitionValueExists(productTuitionValueRequest.ProductTypeId, productTuitionValueRequest.QuantityPeriod, productTuitionValueRequest.TimePeriod);
            if (existsProductTuitionValue)
                throw new HttpRequestException("Já existe essa regra de valor para esse tipo de produto", null, HttpStatusCode.BadRequest);

            var productTuitionValueEntity = _mapper.Map<ProductTuitionValueEntity>(productTuitionValueRequest);

            productTuitionValueEntity.CreatedBy = _email;
            productTuitionValueEntity.CreatedAt = System.DateTime.UtcNow + _timeZone;

            productTuitionValueEntity = await _productTuitionValueRepository.CreateProductTuitionValue(productTuitionValueEntity);

            var productTuitionValueResponse = _mapper.Map<CreateProductTuitionValueResponse>(productTuitionValueEntity);

            return productTuitionValueResponse;
        }

        public async Task<GetProductTuitionValueByIdResponse> GetProductTuitionValueById(int id)
        {
            var productTuitionValueEntity = await _productTuitionValueRepository.GetById(id) ??
               throw new HttpRequestException("Regra de valor do tipo de produto não encontrada", null, HttpStatusCode.NotFound);

            var productTuitionValueResponse = _mapper.Map<GetProductTuitionValueByIdResponse>(productTuitionValueEntity);

            return productTuitionValueResponse;
        }

        public async Task<IEnumerable<GetAllProductTuitionValueByProductTypeIdResponse>> GetAllProductTuitionValueByProductTypeId(int productTypeId)
        {
            var productTuitionValueEntityList = await _productTuitionValueRepository.GetAllByProductTypeId(productTypeId);

            var productTuitionValuesResponseList = _mapper.Map<IEnumerable<GetAllProductTuitionValueByProductTypeIdResponse>>(productTuitionValueEntityList);

            return productTuitionValuesResponseList;
        }

        public async Task<bool> UpdateProductTuitionValue(UpdateProductTuitionValueRequest productTuitionValueRequest, int id)
        {
            var productTuitionValueForUpdate = await _productTuitionValueRepository.GetById(id) ??
                throw new HttpRequestException("Regra de valor do tipo de produto não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionValueRequest.QuantityPeriod != productTuitionValueForUpdate.QuantityPeriod || productTuitionValueRequest.ProductTypeId != productTuitionValueForUpdate.ProductTypeId || productTuitionValueRequest.TimePeriod != productTuitionValueForUpdate.TimePeriod)
            {
                if (productTuitionValueRequest.ProductTypeId != productTuitionValueForUpdate.ProductTypeId)
                {
                    var existsRent = await _productTypeRepository.ProductTypeExists(productTuitionValueRequest.ProductTypeId);
                    if (!existsRent)
                    {
                        throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
                    }
                }

                var existsProductTuitionValue = await _productTuitionValueRepository.ProductTuitionValueExists(productTuitionValueRequest.ProductTypeId, productTuitionValueRequest.QuantityPeriod, productTuitionValueRequest.TimePeriod);
                if (existsProductTuitionValue)
                    throw new HttpRequestException("Já existe essa regra de valor para esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

            productTuitionValueForUpdate.ProductTypeId = productTuitionValueRequest.ProductTypeId;
            productTuitionValueForUpdate.QuantityPeriod = productTuitionValueRequest.QuantityPeriod;
            productTuitionValueForUpdate.TimePeriod = productTuitionValueRequest.TimePeriod;
            productTuitionValueForUpdate.IsDefault = productTuitionValueRequest.IsDefault;
            productTuitionValueForUpdate.Value = productTuitionValueRequest.Value;
            productTuitionValueForUpdate.UpdatedAt = System.DateTime.UtcNow + _timeZone;
            productTuitionValueForUpdate.UpdatedBy = _email;

            if (await _productTuitionValueRepository.UpdateProductTuitionValue(productTuitionValueForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productTuitionValueForDelete = await _productTuitionValueRepository.GetById(id) ??
                throw new HttpRequestException("Regra de valor do tipo de produto não encontrada", null, HttpStatusCode.NotFound);

            if (await _productTuitionValueRepository.DeleteProductTuitionValue(productTuitionValueForDelete) > 0) return true;
            else return false;
        }
    }
}