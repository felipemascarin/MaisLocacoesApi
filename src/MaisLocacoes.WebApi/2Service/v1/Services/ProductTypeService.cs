using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.ProductType;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ProductTypeService(IProductTypeRepository productTypeRepository,
            IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTypeRepository = productTypeRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductTypeResponse> CreateProductType(CreateProductTypeRequest productTypeRequest)
        {
            var existsproductType = await _productTypeRepository.ProductTypeExists(productTypeRequest.Type);
            if (existsproductType)
                throw new HttpRequestException("Tipo de produto já cadastrado", null, HttpStatusCode.BadRequest);

            var productTypeEntity = _mapper.Map<ProductTypeEntity>(productTypeRequest);

            productTypeEntity.CreatedBy = _email;
            productTypeEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            productTypeEntity = await _productTypeRepository.CreateProductType(productTypeEntity);

            var productTypeResponse = _mapper.Map<CreateProductTypeResponse>(productTypeEntity);

            return productTypeResponse;
        }

        public async Task<GetProductTypeByIdResponse> GetProductTypeById(int id)
        {
            var productTypeEntity = await _productTypeRepository.GetById(id) ??
                throw new HttpRequestException("Tipo de produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<GetProductTypeByIdResponse>(productTypeEntity);

            return productTypeResponse;
        }

        public async Task<IEnumerable<GetAllProductTypesResponse>> GetAllProductTypes()
        {
            var productsTypeEntityList = await _productTypeRepository.GetAll();

            var productsTypeResponseList = _mapper.Map<IEnumerable<GetAllProductTypesResponse>>(productsTypeEntityList);

            foreach (var productType in productsTypeResponseList)
            {
                var productLastCreated = await _productRepository.GetTheLastsCreated(productType.Id);
                if (productLastCreated != null)
                {
                    productType.LastCreatedCode = productLastCreated.Code;
                }
            }

            return productsTypeResponseList;
        }

        public async Task UpdateProductType(UpdateProductTypeRequest productTypeRequest, int id)
        {
            var productTypeForUpdate = await _productTypeRepository.GetById(id) ??
                throw new HttpRequestException("Tipo de produto não encontrado", null, HttpStatusCode.NotFound);

            if (productTypeRequest.Type.ToLower() != productTypeForUpdate.Type.ToLower())
            {
                var existsproductType = await _productTypeRepository.ProductTypeExists(productTypeRequest.Type);
                if (existsproductType)
                {
                    throw new HttpRequestException("Não foi possível alterar, já existe esse tipo de produto", null, HttpStatusCode.BadRequest);
                }
            }

            productTypeForUpdate.Type = productTypeRequest.Type;
            productTypeForUpdate.IsManyParts = productTypeRequest.IsManyParts;
            productTypeForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTypeForUpdate.UpdatedBy = _email;

            await _productTypeRepository.UpdateProductType(productTypeForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var productTypeForDelete = await _productTypeRepository.GetById(id) ??
                throw new HttpRequestException("Tipo de produto não encontrado", null, HttpStatusCode.NotFound);

            productTypeForDelete.Deleted = true;
            productTypeForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productTypeForDelete.UpdatedBy = _email;

            await _productTypeRepository.UpdateProductType(productTypeForDelete);
        }
    }
}