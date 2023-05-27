using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _productTypeRepository = productTypeRepository;
        }

        public async Task<ProductResponse> CreateProduct(ProductRequest productRequest)
        {
            var existsProduct = await _productRepository.GetByTypeCode(productRequest.ProductTypeId, productRequest.Code);

            if (existsProduct != null)
                throw new HttpRequestException("Produto já cadastrado", null, HttpStatusCode.BadRequest);

            var existsProductType = await _productTypeRepository.ProductTypeExists(productRequest.ProductTypeId);
            if (!existsProductType)
            {
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

            var productEntity = _mapper.Map<ProductEntity>(productRequest);

            productEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productEntity = await _productRepository.CreateProduct(productEntity);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            return productResponse;
        }

        public async Task<ProductResponse> GetById(int id)
        {
            var productEntity = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            return productResponse;
        }

        public async Task<ProductResponse> GetByTypeCode(int typeId, string code)
        {
            var productEntity = await _productRepository.GetByTypeCode(typeId, code) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            return productResponse;
        }

        public async Task<bool> UpdateProduct(ProductRequest productRequest, int id)
        {
            var productForUpdate = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.BadRequest);

            if (productRequest.ProductTypeId != productForUpdate.ProductTypeId || productRequest.Code.ToLower() != productForUpdate.Code.ToLower())
            {
                if (productRequest.ProductTypeId != productForUpdate.ProductTypeId)
                {
                    var existsProductType = await _productTypeRepository.ProductTypeExists(productRequest.ProductTypeId);
                    if (!existsProductType)
                    {
                        throw new HttpRequestException("Esse tipo de produto não existe", null, HttpStatusCode.BadRequest);
                    }
                }

                var existsTypeCode = await _productRepository.ProductExists(productRequest.ProductTypeId, productRequest.Code);
                if (existsTypeCode)
                {
                    throw new HttpRequestException("O código para esse tipo do produto já existe", null, HttpStatusCode.BadRequest);
                }
            }

            productForUpdate.ProductTypeId = productRequest.ProductTypeId;
            productForUpdate.Code = productRequest.Code;
            productForUpdate.SupplierId = productRequest.SupplierId;
            productForUpdate.Description = productRequest.Description;
            productForUpdate.DateBought = productRequest.DateBought;
            productForUpdate.BoughtValue = productRequest.BoughtValue;
            productForUpdate.CurrentRentedPlaceId = productRequest.CurrentRentedPlaceId;
            productForUpdate.Parts = productRequest.Parts;
            productForUpdate.UpdatedAt = System.DateTime.UtcNow;
            productForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productRepository.UpdateProduct(productForUpdate) > 0) return true;
            else return false;
        }
    }
}