using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class ProductTuitionService : IProductTuitionService
    {
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductTuitionService(IProductTuitionRepository productTuitionRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _productTuitionRepository = productTuitionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductTuitionResponse> CreateProductTuition(ProductTuitionRequest productTuitionRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductTuitionResponse> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateProductTuition(ProductTuitionRequest productTuitionRequest, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteById(int id)
        {
            var productTuitionForDelete = await _productTuitionRepository.GetById(id) ??
                throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

            productTuitionForDelete.Deleted = true;
            productTuitionForDelete.UpdatedAt = System.DateTime.UtcNow;
            productTuitionForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _productTuitionRepository.UpdateProductTuition(productTuitionForDelete) > 0) return true;
            else return false;
        }
    }
}