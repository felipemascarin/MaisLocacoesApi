using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
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

        public ProductWasteService(IProductWasteRepository productWasteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productWasteRepository = productWasteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductWasteResponse> CreateProductWaste(ProductWasteRequest productWasteRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductWasteResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductWaste(ProductWasteRequest productWasteRequest, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteById(int id)
        {
            var productWasteForDelete = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            productWasteForDelete.Deleted = true;
            productWasteForDelete.UpdatedAt = System.DateTime.UtcNow;
            productWasteForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productWasteRepository.UpdateProductWaste(productWasteForDelete) > 0) return true;
            else return false;
        }
    }
}