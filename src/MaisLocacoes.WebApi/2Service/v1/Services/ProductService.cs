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
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ProductService(IProductRepository productRepository,
            IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductResponse> CreateProduct(CreateProductRequest productRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productRequest = TimeZoneConverter<CreateProductRequest>.ConvertToTimeZoneLocal(productRequest, _timeZone);

            var existsProductType = await _productTypeRepository.GetById(productRequest.ProductTypeId) ??
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);

            var existsProduct = await _productRepository.GetByTypeCode(productRequest.ProductTypeId, productRequest.Code);
            if (existsProduct != null)
                throw new HttpRequestException("Produto já cadastrado", null, HttpStatusCode.BadRequest);

            if (productRequest.RentedParts > productRequest.Parts)
                throw new HttpRequestException("Não é possível alugar mais peças do que existe no produto", null, HttpStatusCode.BadRequest);

            var productEntity = _mapper.Map<ProductEntity>(productRequest);

            productEntity.ProductTypeEntity = existsProductType;

            productEntity.CreatedBy = _email;
            productEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            productEntity = await _productRepository.CreateProduct(productEntity);

            var productTypeResponse = _mapper.Map<CreateProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<CreateProductResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<GetProductByIdResponse> GetProductById(int id)
        {
            var productEntity = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<CreateProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<GetProductByIdResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<GetProductByTypeCodeResponse> GetProductByTypeCode(int typeId, string code)
        {
            var productEntity = await _productRepository.GetByTypeCode(typeId, code) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<CreateProductTypeResponse>(productEntity.ProductTypeEntity);

            var productResponse = _mapper.Map<GetProductByTypeCodeResponse>(productEntity);

            productResponse.ProductType = productTypeResponse;

            return productResponse;
        }

        public async Task<IEnumerable<GetProductsByPageResponse>> GetProductsByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var productsEntityList = await _productRepository.GetProductsByPage(items, page, query);

            var productsEntityListLenght = productsEntityList.ToList().Count;

            var productsResponseList = _mapper.Map<IEnumerable<GetProductsByPageResponse>>(productsEntityList);

            for (int i = 0; i < productsEntityListLenght; i++)
            {
                productsResponseList.ElementAt(i).ProductType = _mapper.Map<CreateProductTypeResponse>(productsEntityList.ElementAt(i).ProductTypeEntity);
            }

            return productsResponseList;
        }

        public async Task<IEnumerable<GetProductsForRentResponse>> GetProductsForRent(int productTypeId)
        {
            var productForRentDtoResponse = await _productRepository.GetProductsForRent(productTypeId);

            var productForRentResponse = new List<GetProductsForRentResponse>();

            foreach (var item in productForRentDtoResponse)
            {
                productForRentResponse.Add(new GetProductsForRentResponse
                {
                    Code = item.Code,
                    FreeParts = item.Parts - item.RentedParts
                });
            }

            return productForRentResponse.OrderBy(p => p.Code);
        }

        public async Task UpdateProduct(UpdateProductRequest productRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productRequest = TimeZoneConverter<UpdateProductRequest>.ConvertToTimeZoneLocal(productRequest, _timeZone);

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
            productForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForUpdate.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForUpdate);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var productForUpdate = await _productRepository.GetById(id) ??
                    throw new HttpRequestException("produto não encontrado", null, HttpStatusCode.NotFound);

            productForUpdate.Status = status;
            productForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForUpdate.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var productForDelete = await _productRepository.GetById(id) ??
                throw new HttpRequestException("Produto não encontrado", null, HttpStatusCode.NotFound);

            productForDelete.Deleted = true;
            productForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productForDelete.UpdatedBy = _email;

            await _productRepository.UpdateProduct(productForDelete);
        }
    }
}