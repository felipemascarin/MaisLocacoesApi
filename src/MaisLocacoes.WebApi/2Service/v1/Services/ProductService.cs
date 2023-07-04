using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.Repository;
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
            var existsProductType = await _productTypeRepository.GetById(productRequest.ProductTypeId) ??
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            var existsProduct = await _productRepository.GetByTypeCode(productRequest.ProductTypeId, productRequest.Code);
            if (existsProduct != null)
                throw new HttpRequestException("Produto já cadastrado", null, HttpStatusCode.BadRequest);

            if (productRequest.RentedParts > productRequest.Parts)
                throw new HttpRequestException("Não é possível alugar mais peças do que existe no produto", null, HttpStatusCode.BadRequest);

            var productEntity = _mapper.Map<ProductEntity>(productRequest);

            productEntity.ProductTypeEntity = existsProductType;

            productEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productEntity = await _productRepository.CreateProduct(productEntity);

            var productTypeResponse = _mapper.Map<ProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<ProductResponse> GetById(int id)
        {
            var productEntity = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<ProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<ProductResponse> GetByTypeCode(int typeId, string code)
        {
            var productEntity = await _productRepository.GetByTypeCode(typeId, code) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<ProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<ProductResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<IEnumerable<ProductResponse>> GetProductsByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var productsEntityList = await _productRepository.GetProductsByPage(items, page, query);

            var productsEntityListLenght = productsEntityList.ToList().Count;

            var productsResponseList = _mapper.Map<IEnumerable<ProductResponse>>(productsEntityList);

            for (int i = 0; i < productsEntityListLenght; i++)
            {
                productsResponseList.ElementAt(i).ProductType = _mapper.Map<ProductTypeResponse>(productsEntityList.ElementAt(i).ProductTypeEntity);
            }

            return productsResponseList;
        }

        public async Task<IEnumerable<GetProductForRentResponse>> GetProductsForRent(int productTypeId)
        {
            var productForRentDtoResponse = await _productRepository.GetProductsForRent(productTypeId);

            var productForRentResponse = new List<GetProductForRentResponse>();

            foreach (var item in productForRentDtoResponse)
            {
                productForRentResponse.Add(new GetProductForRentResponse
                {
                    Code = item.Code,
                    FreeParts = item.Parts - item.RentedParts
                });
            }

            return productForRentResponse.OrderBy(p => p.Code);
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
                        throw new HttpRequestException("Esse tipo de produto não existe", null, HttpStatusCode.BadRequest);
                }

                var existsTypeCode = await _productRepository.ProductExists(productRequest.ProductTypeId, productRequest.Code);
                if (existsTypeCode)
                    throw new HttpRequestException("O código para esse tipo do produto já existe", null, HttpStatusCode.BadRequest);
            }

            if (productRequest.RentedParts > productRequest.Parts)
                throw new HttpRequestException("Não é possível alugar mais peças do que existe no produto", null, HttpStatusCode.BadRequest);

            productForUpdate.ProductTypeId = productRequest.ProductTypeId;
            productForUpdate.Code = productRequest.Code;
            productForUpdate.SupplierId = productRequest.SupplierId;
            productForUpdate.Description = productRequest.Description;
            productForUpdate.DateBought = productRequest.DateBought;
            productForUpdate.BoughtValue = productRequest.BoughtValue;
            productForUpdate.CurrentRentedPlaceId = productRequest.CurrentRentedPlaceId;
            productForUpdate.Parts = productRequest.Parts;
            productForUpdate.RentedParts = productRequest.RentedParts;
            productForUpdate.UpdatedAt = System.DateTime.Now;
            productForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productRepository.UpdateProduct(productForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var productForUpdate = await _productRepository.GetById(id) ??
                    throw new HttpRequestException("produto não encontrado", null, HttpStatusCode.NotFound);

            productForUpdate.Status = status;
            productForUpdate.UpdatedAt = System.DateTime.Now;
            productForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productRepository.UpdateProduct(productForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productForDelete = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            productForDelete.Deleted = true;
            productForDelete.UpdatedAt = System.DateTime.Now;
            productForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productRepository.UpdateProduct(productForDelete) > 0) return true;
            else return false;
        }
    }
}