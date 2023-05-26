using AutoMapper;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.IRepository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IDeletionsRepository _deletionsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository,
            IRentRepository rentRepository,
            IDeletionsRepository deletionsRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _rentRepository = rentRepository;
            _deletionsRepository = deletionsRepository;
        }

        public async Task<CreateBillResponse> CreateBill(BillRequest billRequest)
        {
            var existsRent = await _rentRepository.RentExists(billRequest.RentId);
            if (!existsRent)
            {
                throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
            }

            var billEntity = _mapper.Map<BillEntity>(billRequest);

            billEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            billEntity = await _billRepository.CreateBill(billEntity);

            var billResponse = _mapper.Map<CreateBillResponse>(billEntity);

            return billResponse;
        }

        public async Task<GetBillResponse> GetById(int id)
        {
            var billEntity = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var billResponse = _mapper.Map<GetBillResponse>(billEntity);

            return billResponse;
        }

        public async Task<bool> UpdateBill(BillRequest billRequest, int id)
        {
            var billForUpdate = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (billRequest.RentId != billForUpdate.RentId)
            {
                var existsRent = await _rentRepository.RentExists(billRequest.RentId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe essa locação", null, HttpStatusCode.BadRequest);
                }
            }

            billForUpdate.RentId = billRequest.RentId;
            billForUpdate.Value = billRequest.Value;
            billForUpdate.PayDate = billRequest.PayDate;
            billForUpdate.DueDate = billRequest.DueDate;
            billForUpdate.NfIdFireBase = billRequest.NfIdFireBase;
            billForUpdate.PaymentMode = billRequest.PaymentMode;
            billForUpdate.UpdatedAt = System.DateTime.UtcNow;
            billForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _billRepository.UpdateBill(billForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var billForUpdate = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrado", null, HttpStatusCode.NotFound);

            billForUpdate.Status = status;
            billForUpdate.UpdatedAt = System.DateTime.UtcNow;
            billForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _billRepository.UpdateBill(billForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var billEntity = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var billForDelete = _mapper.Map<BillsDeletions>(billEntity);
            billForDelete.DeletedAt = System.DateTime.UtcNow;
            billForDelete.DeletedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            await _deletionsRepository.CreateBillsDeletions(billForDelete);

            if (await _deletionsRepository.DeleteBill(billEntity) > 0) return true;
            else return false;
        }
    }
}