using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

namespace Service.v1.Services
{
    public class ProductWasteService : IProductWasteService
    {
        private readonly IProductWasteRepository _productWasteRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public ProductWasteService(IProductWasteRepository productWasteRepository,
            IProductRepository productRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _productWasteRepository = productWasteRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateProductWasteResponse> CreateProductWaste(CreateProductWasteRequest productWasteRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productWasteRequest = TimeZoneConverter<CreateProductWasteRequest>.ConvertToTimeZoneLocal(productWasteRequest, _timeZone);

            var existsproduct = await _productRepository.ProductExists(productWasteRequest.ProductId);
            if (!existsproduct)
            {
                throw new HttpRequestException("Não existe esse produto", null, HttpStatusCode.BadRequest);
            }

            var productWasteEntity = _mapper.Map<ProductWasteEntity>(productWasteRequest);

            productWasteEntity.CreatedBy = _email;
            productWasteEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            productWasteEntity = await _productWasteRepository.CreateProductWaste(productWasteEntity);

            var productWasteResponse = _mapper.Map<CreateProductWasteResponse>(productWasteEntity);

            return productWasteResponse;
        }

        public async Task<GetProductWasteByIdResponse> GetProductWasteById(int id)
        {
            var productWasteEntity = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            var productWasteResponse = _mapper.Map<GetProductWasteByIdResponse>(productWasteEntity);

            return productWasteResponse;
        }

        public async Task<GetAllProductWastesByProductIdResponse> GetAllProductWastesByProductId(int productId)
        {
            var productWastesEntityList = (await _productWasteRepository.GetAllByProductId(productId)).ToList();

            var productWastesResponse = new GetAllProductWastesByProductIdResponse()
            {
                ProductsWastes = _mapper.Map<List<CreateProductWasteResponse>>(productWastesEntityList),
                TotalWastesValue = 0
            };

            productWastesEntityList.ForEach(p => productWastesResponse.TotalWastesValue += p.Value);

            return productWastesResponse;
        }

        public async Task<IEnumerable<GetProductWastesByPageResponse>> GetProductWastesByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var productWastesEntityList = await _productWasteRepository.GetProductWastesByPage(items, page, query);

            var productWastesEntityListLenght = productWastesEntityList.ToList().Count;

            var productWastesResponseList = _mapper.Map<IEnumerable<GetProductWastesByPageResponse>>(productWastesEntityList);

            return productWastesResponseList;
        }

        public async Task<bool> UpdateProductWaste(UpdateProductWasteRequest productWasteRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            productWasteRequest = TimeZoneConverter<UpdateProductWasteRequest>.ConvertToTimeZoneLocal(productWasteRequest, _timeZone);

            var productWasteForUpdate = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            if (productWasteRequest.ProductId != productWasteForUpdate.ProductId)
            {
                var existsproduct = await _productRepository.ProductExists(productWasteRequest.ProductId);
                if (!existsproduct)
                {
                    throw new HttpRequestException("Não existe esse produto", null, HttpStatusCode.BadRequest);
                }
            }

            productWasteForUpdate.ProductId = productWasteRequest.ProductId;
            productWasteForUpdate.Description = productWasteRequest.Description;
            productWasteForUpdate.Value = productWasteRequest.Value;
            productWasteForUpdate.Date = productWasteRequest.Date;
            productWasteForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productWasteForUpdate.UpdatedBy = _email;

            if (await _productWasteRepository.UpdateProductWaste(productWasteForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var productWasteForDelete = await _productWasteRepository.GetById(id) ??
                throw new HttpRequestException("Gasto de produto não encontrado", null, HttpStatusCode.NotFound);

            productWasteForDelete.Deleted = true;
            productWasteForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            productWasteForDelete.UpdatedBy = _email;

            if (await _productWasteRepository.UpdateProductWaste(productWasteForDelete) > 0) return true;
            else return false;
        }
    }
}