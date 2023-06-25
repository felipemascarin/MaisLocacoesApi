using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductTypeService(IProductTypeRepository productTypeRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTypeRepository = productTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ProductTypeResponse> CreateProductType(ProductTypeRequest productTypeRequest)
        {
            var existsproductType = await _productTypeRepository.ProductTypeExists(productTypeRequest.Type);
            if (existsproductType)
                throw new HttpRequestException("Tipo de produto já cadastrado", null, HttpStatusCode.BadRequest);

            var productTypeEntity = _mapper.Map<ProductTypeEntity>(productTypeRequest);

            productTypeEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productTypeEntity = await _productTypeRepository.CreateProductType(productTypeEntity);

            var productTypeResponse = _mapper.Map<ProductTypeResponse>(productTypeEntity);

            return productTypeResponse;
        }

        public async Task<ProductTypeResponse> GetById(int id)
        {
            var productTypeEntity = await _productTypeRepository.GetById(id) ??
                throw new HttpRequestException("Tipo de produto não encontrado", null, HttpStatusCode.NotFound);

            var productTypeResponse = _mapper.Map<ProductTypeResponse>(productTypeEntity);

            return productTypeResponse;
        }

        public async Task<IEnumerable<ProductTypeResponse>> GetAll()
        {
            var productsTypeEntityList = await _productTypeRepository.GetAll();

            var productsTypeResponseList = _mapper.Map<IEnumerable<ProductTypeResponse>>(productsTypeEntityList);

            return productsTypeResponseList;
        }

        public async Task<bool> UpdateProductType(ProductTypeRequest productTypeRequest, int id)
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
            productTypeForUpdate.UpdatedAt = System.DateTime.Now;
            productTypeForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTypeRepository.UpdateProductType(productTypeForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productTypeForDelete = await _productTypeRepository.GetById(id) ??
                throw new HttpRequestException("Tipo de produto não encontrado", null, HttpStatusCode.NotFound);

            productTypeForDelete.Deleted = true;
            productTypeForDelete.UpdatedAt = System.DateTime.Now;
            productTypeForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTypeRepository.UpdateProductType(productTypeForDelete) > 0) return true;
            else return false;
        }
    }
}