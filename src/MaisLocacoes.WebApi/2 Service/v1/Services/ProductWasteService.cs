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
    public class ProductWasteService : IProductWasteService
    {
        private readonly IProductWasteRepository _productWasteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProductWasteService(IProductWasteRepository productWasteRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productWasteRepository = productWasteRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ProductWasteResponse> CreateProductWaste(ProductWasteRequest productWasteRequest)
        {
            var productWasteEntity = _mapper.Map<ProductWasteEntity>(productWasteRequest);

            productWasteEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            productWasteEntity = await _productWasteRepository.CreateProductWaste(productWasteEntity);

            var productWasteResponse = _mapper.Map<ProductWasteResponse>(productWasteEntity);

            return productWasteResponse;
        }

        public async Task<ProductWasteResponse> GetById(int id)
        {
            var productWasteEntity = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            var productWasteResponse = _mapper.Map<ProductWasteResponse>(productWasteEntity);

            return productWasteResponse;
        }

        public async Task<bool> UpdateProductWaste(ProductWasteRequest productWasteRequest, int id)
        {
            var productWasteForUpdate = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            productWasteForUpdate.ProductCode = productWasteRequest.ProductCode;
            productWasteForUpdate.ProductType = productWasteRequest.ProductType;
            productWasteForUpdate.Description = productWasteRequest.Description;
            productWasteForUpdate.Value = productWasteRequest.Value;
            productWasteForUpdate.Date = productWasteRequest.Date;
            productWasteForUpdate.UpdatedAt = System.DateTime.UtcNow;
            productWasteForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productWasteRepository.UpdateProductWaste(productWasteForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productWasteForDelete = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            productWasteForDelete.Deleted = true;
            productWasteForDelete.UpdatedAt = System.DateTime.UtcNow;
            productWasteForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productWasteRepository.UpdateProductWaste(productWasteForDelete) > 0) return true;
            else return false;
        }
    }
}