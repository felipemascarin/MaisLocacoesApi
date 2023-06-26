using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.IRepository.UserSchema;
using Repository.v1.Repository.UserSchema;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class BillService : IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IRentRepository _rentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository,
            IProductTuitionRepository productTuitionRepository,
            ICompanyRepository companyRepository,
            IRentRepository rentRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _productTuitionRepository = productTuitionRepository;
            _companyRepository = companyRepository;
            _rentRepository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BillResponse> CreateBill(BillRequest billRequest)
        {
            var rentExists = await _rentRepository.RentExists(billRequest.RentId);
            if (!rentExists)
                throw new HttpRequestException("Não existe essa Locação", null, HttpStatusCode.BadRequest);

            if (billRequest.ProductTuitionId != null)
            {
                var existsProductTuition = await _productTuitionRepository.ProductTuitionExists(billRequest.ProductTuitionId);
                if (!existsProductTuition)
                    throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
            }

            var billEntity = _mapper.Map<BillEntity>(billRequest);

            billEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            billEntity = await _billRepository.CreateBill(billEntity);

            var billResponse = _mapper.Map<BillResponse>(billEntity);

            return billResponse;
        }

        public async Task<BillResponse> GetById(int id)
        {
            var billEntity = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var billResponse = _mapper.Map<BillResponse>(billEntity);

            return billResponse;
        }

        public async Task<IEnumerable<GetBillProductTypeForRentResponse>> GetByRentId(int rentId)
        {
            var billsEntityList = await _billRepository.GetByRentId(rentId);

            var productTuitionEntityList = await _productTuitionRepository.GetAllByRentId(rentId);

            var billsResponseList = _mapper.Map<IEnumerable<GetBillProductTypeForRentResponse>>(billsEntityList);

            foreach (var bill in billsResponseList)
            {
                if (bill.ProductTuitionId != null)
                {
                    bill.ProductCode = productTuitionEntityList.FirstOrDefault(p => p.Id == bill.ProductTuitionId).ProductCode;
                    bill.ProductTuitionParts = productTuitionEntityList.FirstOrDefault(p => p.Id == bill.ProductTuitionId).Parts;
                    bill.ProductType = _mapper.Map<ProductTypeResponse>(productTuitionEntityList.FirstOrDefault(p => p.Id == bill.ProductTuitionId).ProductTypeEntity);
                }
            }

            return billsResponseList;
        }

        public async Task<IEnumerable<BillResponse>> GetDuedBills()
        {
            var company = await _companyRepository.GetByCnpj(JwtManager.GetSchemaByToken(_httpContextAccessor));

            var billsEntityList = await _billRepository.GetDuedBills(company.NotifyDaysBefore);

            var billsResponseList = _mapper.Map<IEnumerable<BillResponse>>(billsEntityList);

            return billsResponseList;
        }

        public async Task<bool> UpdateBill(BillRequest billRequest, int id)
        {
            var billForUpdate = await _billRepository.GetById(id) ??
               throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (billRequest.RentId != billForUpdate.RentId)
            {
                var rentExists = await _rentRepository.RentExists(billRequest.RentId);
                if (!rentExists)
                    throw new HttpRequestException("Não existe essa Locação", null, HttpStatusCode.BadRequest);
            }

            if (billRequest.ProductTuitionId != billForUpdate.ProductTuitionId && billRequest.ProductTuitionId != null)
            {
                var existsRent = await _productTuitionRepository.ProductTuitionExists(billRequest.ProductTuitionId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
                }
            }

            billForUpdate.ProductTuitionId = billRequest.ProductTuitionId;
            billForUpdate.Value = billRequest.Value;
            billForUpdate.PayDate = billRequest.PayDate;
            billForUpdate.DueDate = billRequest.DueDate;
            billForUpdate.NfIdFireBase = billRequest.NfIdFireBase;
            billForUpdate.PaymentMode = billRequest.PaymentMode;
            billForUpdate.Description = billRequest.Description;
            billForUpdate.UpdatedAt = System.DateTime.Now;
            billForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _billRepository.UpdateBill(billForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, string paymentMode, int id)
        {
            var billForUpdate = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (status.ToLower() == BillStatus.BillStatusEnum.ElementAt(1) && paymentMode == null)
                throw new HttpRequestException("Modo de pagamento deve ser inserido para status de fatura paga", null, HttpStatusCode.BadRequest);

            if (status.ToLower() != BillStatus.BillStatusEnum.ElementAt(1))
                paymentMode = null;

            if (paymentMode != null && !PaymentModes.PaymentModesEnum.Contains(paymentMode.ToLower()))
                throw new HttpRequestException("Não existe esse modo de pagamento", null, HttpStatusCode.BadRequest);

            billForUpdate.Status = status;
            billForUpdate.PaymentMode = paymentMode;
            billForUpdate.UpdatedAt = System.DateTime.Now;
            billForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _billRepository.UpdateBill(billForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var billForDelete = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            billForDelete.Deleted = true;
            billForDelete.UpdatedAt = System.DateTime.Now;
            billForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _billRepository.UpdateBill(billForDelete) > 0) return true;
            else return false;
        }
    }
}