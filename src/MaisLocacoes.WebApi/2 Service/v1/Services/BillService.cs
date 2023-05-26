using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository,
            IRentRepository rentRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _rentRepository = rentRepository;
        }

        public async Task<CreateBillResponse> CreateBill(BillRequest billRequest)
        {
            _ = await _rentRepository.GetById(billRequest.RentId) ??
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);

            var billEntity = _mapper.Map<BillEntity>(billRequest);

            billEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            billEntity = await _billRepository.CreateBill(billEntity);

            var billResponse = _mapper.Map<CreateBillResponse>(billEntity);

            return billResponse;
        }
    }
}