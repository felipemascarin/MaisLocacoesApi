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
    public class ProductTuitionService : IProductTuitionService
    {
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IRentRepository rentRepository,
            IProductTypeRepository productTypeRepositor,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productTuitionRepository = productTuitionRepository;
            _rentRepository = rentRepository;
            _productTypeRepository = productTypeRepositor;
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

            var existsproductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId);
            if (!existsproductType)
            {
                throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
            }

            var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId, productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode);
            if (existsproductTuition)
            {
                throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);
            }

            var productTuitionEntity = _mapper.Map<ProductTuitionEntity>(productTuitionRequest);

            productTuitionEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productTuitionEntity = await _productTuitionRepository.CreateProductTuition(productTuitionEntity);

            var productTuitionResponse = _mapper.Map<ProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<ProductTuitionResponse> GetById(int id)
        {
            var productTuitionEntity = await _productTuitionRepository.GetById(id) ??
               throw new HttpRequestException("Fatura do produto não encontrada", null, HttpStatusCode.NotFound);

            var productTuitionResponse = _mapper.Map<ProductTuitionResponse>(productTuitionEntity);

            return productTuitionResponse;
        }

        public async Task<IEnumerable<ProductTuitionResponse>> GetAllByRentId(int rentId)
        {
            var productTuitionEntityList = await _productTuitionRepository.GetAllByRentId(rentId);

            var productTuitionResponse = _mapper.Map<IEnumerable<ProductTuitionResponse>>(productTuitionEntityList);

            return productTuitionResponse;
        }

        public async Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id)
        {
            var productTuitionForUpdate = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (productTuitionRequest.RentId != productTuitionForUpdate.RentId || productTuitionRequest.ProductTypeId != productTuitionForUpdate.ProductTypeId || productTuitionRequest.ProductCode.ToLower() != productTuitionForUpdate.ProductCode.ToLower())
            {
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
                    var existsproductType = await _productTypeRepository.ProductTypeExists(productTuitionRequest.ProductTypeId);
                    if (!existsproductType)
                    {
                        throw new HttpRequestException("Não existe esse tipo de produto", null, HttpStatusCode.BadRequest);
                    }
                }

                var existsproductTuition = await _productTuitionRepository.ProductTuitionExists(productTuitionRequest.RentId, productTuitionRequest.ProductTypeId, productTuitionRequest.ProductCode);
                if (existsproductTuition)
                {
                    throw new HttpRequestException("Já existe esse produto nessa locação", null, HttpStatusCode.BadRequest);
                }
            }

            productTuitionForUpdate.RentId = productTuitionRequest.RentId;
            productTuitionForUpdate.ProductTypeId = productTuitionRequest.ProductTypeId;
            productTuitionForUpdate.ProductCode = productTuitionRequest.ProductCode;
            productTuitionForUpdate.Value = productTuitionRequest.Value;
            productTuitionForUpdate.InitialDateTime = productTuitionRequest.InitialDateTime;
            productTuitionForUpdate.FinalDateTime = productTuitionRequest.FinalDateTime;
            productTuitionForUpdate.Parts = productTuitionRequest.Parts;
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
    }
}