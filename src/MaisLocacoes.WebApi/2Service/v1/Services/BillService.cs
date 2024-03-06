using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Bill;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Repository.v1.IRepository.UserSchema;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;

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
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

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
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateBillResponse> CreateBill(CreateBillRequest billRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            billRequest = TimeZoneConverter<CreateBillRequest>.ConvertToTimeZoneLocal(billRequest, _timeZone);

            var rentExists = await _rentRepository.RentExists(billRequest.RentId.Value);
            if (!rentExists)
                throw new HttpRequestException("Não existe essa Locação", null, HttpStatusCode.BadRequest);

            if (billRequest.ProductTuitionId != null)
            {
                var existsProductTuition = await _productTuitionRepository.ProductTuitionExists(billRequest.ProductTuitionId);
                if (!existsProductTuition)
                    throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
            }

            var billEntity = _mapper.Map<BillEntity>(billRequest);

            billEntity.CreatedBy = _email;
            billEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            billEntity = await _billRepository.CreateBill(billEntity);

            var billResponse = _mapper.Map<CreateBillResponse>(billEntity);

            return billResponse;
        }

        public async Task<GetBillByIdResponse> GetBillById(int id)
        {
            var billEntity = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var billResponse = _mapper.Map<GetBillByIdResponse>(billEntity);

            return billResponse;
        }

        public async Task<GetBillForTaxInvoiceResponse> GetBillForTaxInvoice(int billId)
        {
            var billEntity = await _billRepository.GetForTaxInvoice(billId) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (billEntity.InvoiceId == null)
            {
                billEntity.InvoiceId = (await _billRepository.GetTheLastInvoiceId()) + 1;

                if (billEntity.InvoiceEmittedDate == null)
                    billEntity.InvoiceEmittedDate = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

                await _billRepository.UpdateBill(billEntity);
            }

            var companyEntity = await _companyRepository.GetByCnpj(JwtManager.GetCnpjByToken(_httpContextAccessor)) ??
                throw new HttpRequestException("Empresa não encontrada", null, HttpStatusCode.NotFound);

            string productCode = null;
            int? productTuitionParts = null;
            GetBillForTaxInvoiceResponse.ProductTypeResponse productType = null;

            if (billEntity.ProductTuitionId != null)
            {
                var productTuitionEntity = await _productTuitionRepository.GetById(billEntity.ProductTuitionId.Value) ??
                    throw new HttpRequestException("Fatura de produto ProducTuition não encontrada no banco para uma das faturas", null, HttpStatusCode.NotFound);
                productCode = productTuitionEntity.ProductCode;
                productTuitionParts = productTuitionEntity.Parts;
                productType = _mapper.Map<GetBillForTaxInvoiceResponse.ProductTypeResponse>(productTuitionEntity.ProductType);
            }

            var billForTaxInvoiceResponse = _mapper.Map<GetBillForTaxInvoiceResponse>(billEntity);

            billForTaxInvoiceResponse.Company = _mapper.Map<GetBillForTaxInvoiceResponse.CompanyResponse>(companyEntity);
            billForTaxInvoiceResponse.ProductType = productType;
            billForTaxInvoiceResponse.ProductCode = productCode;
            billForTaxInvoiceResponse.ProductTuitionParts = productTuitionParts;

            return billForTaxInvoiceResponse;
        }

        public async Task<IEnumerable<GetBillByRentIdResponse>> GetBillByRentId(int rentId)
        {
            var billsEntityList = await _billRepository.GetByRentId(rentId);

            var billsResponseList = _mapper.Map<IEnumerable<GetBillByRentIdResponse>>(billsEntityList);

            foreach (var bill in billsResponseList)
            {
                if (bill.ProductTuitionId != null)
                {
                    var productTuition = billsEntityList.FirstOrDefault(p => p.ProductTuitionId == bill.ProductTuitionId).ProductTuition;

                    bill.ProductCode = productTuition.ProductCode;
                    bill.ProductTuitionParts = productTuition.Parts;
                    bill.ProductType = _mapper.Map<GetBillByRentIdResponse.ProductTypeResponse>(productTuition.ProductType);
                }
            }

            billsResponseList.OrderBy(b => b.ProductCode).ThenBy(b => b.Order);

            return billsResponseList;
        }

        public async Task<IEnumerable<GetDuedsBillsResponse>> GetDuedBills()
        {
            var company = await _companyRepository.GetByCnpj(JwtManager.GetCnpjByToken(_httpContextAccessor));

            var todayDate = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            var billsEntityList = await _billRepository.GetDuedBills(company.NotifyDaysBefore, todayDate);

            var billDtoList = new List<GetDuedsBillsResponse>();

            foreach (var bill in billsEntityList)
            {
                string productTypeName;
                bool? isManyParts;
                string productCode;
                int? parts;

                if (bill.ProductTuitionId == null)
                {
                    productTypeName = null;
                    isManyParts = null;
                    productCode = null;
                    parts = null;
                }
                else
                {
                    productTypeName = bill.ProductTuition.ProductType.Type;
                    isManyParts = bill.ProductTuition.ProductType.IsManyParts;
                    productCode = bill.ProductTuition.ProductCode;
                    parts = bill.ProductTuition.Parts;
                }

                var billDto = new GetDuedsBillsResponse()
                {
                    ClientName = bill.Rent.Client.ClientName,
                    DueDate = bill.DueDate,
                    Value = bill.Value,
                    ClientPhone = bill.Rent.Client.Cel,
                    RentId = bill.Rent.Id,
                    BillId = bill.Id,
                    BillDescription = bill.Description,
                    InvoiceId = bill.InvoiceId,
                    ProductTypeName = productTypeName,
                    ProductCode = productCode,
                    Parts = parts,
                    IsManyParts = isManyParts,
                    InvoiceEmittedDate = bill.InvoiceEmittedDate
                };

                billDtoList.Add(billDto);
            }

            return billDtoList;
        }

        public async Task<IEnumerable<GetAllBillsDebtsResponse>> GetAllBillsDebts()
        {
            var billsEntityList = await _billRepository.GetAllDebts() ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            var billDtoList = new List<GetAllBillsDebtsResponse>();

            foreach (var bill in billsEntityList)
            {
                string productTypeName;
                bool? isManyParts;
                string productCode;
                int? parts;

                if (bill.ProductTuitionId == null)
                {
                    productTypeName = null;
                    isManyParts = null;
                    productCode = null;
                    parts = null;
                }
                else
                {
                    productTypeName = bill.Rent.Client.Type;
                    isManyParts = bill.ProductTuition.ProductType.IsManyParts;
                    productCode = bill.ProductTuition.ProductCode;
                    parts = bill.ProductTuition.Parts;
                }

                var billDto = new GetAllBillsDebtsResponse()
                {
                    ClientName = bill.Rent.Client.ClientName,
                    DueDate = bill.DueDate,
                    Value = bill.Value,
                    ClientPhone = bill.Rent.Client.Cel,
                    RentId = bill.Rent.Id,
                    BillId = bill.Id,
                    BillDescription = bill.Description,
                    InvoiceId = bill.InvoiceId,
                    ProductTypeName = productTypeName,
                    ProductCode = productCode,
                    Parts = parts,
                    IsManyParts = isManyParts,
                    InvoiceEmittedDate = bill.InvoiceEmittedDate
                };

                billDtoList.Add(billDto);
            }

            return billDtoList;
        }

        public async Task UpdateBill(UpdateBillRequest billRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            billRequest = TimeZoneConverter<UpdateBillRequest>.ConvertToTimeZoneLocal(billRequest, _timeZone);

            var billForUpdate = await _billRepository.GetById(id) ??
               throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (billForUpdate.InvoiceId != null)
                throw new HttpRequestException("Fatura não pode ser editada, pois possui Nota Fiscal", null, HttpStatusCode.NotFound);

            //Se estiver sendo atualizada a locação que a bill pertence, é verificado a mesma existe
            if (billRequest.RentId != billForUpdate.RentId)
            {
                var rentExists = await _rentRepository.RentExists(billRequest.RentId.Value);
                if (!rentExists)
                    throw new HttpRequestException("Não existe essa Locação", null, HttpStatusCode.BadRequest);
            }

            //Se estiver sendo atualizado o ProductTuition que a bill pertence, é verificado o mesmo existe
            if (billRequest.ProductTuitionId != billForUpdate.ProductTuitionId && billRequest.ProductTuitionId != null)
            {
                var existsRent = await _productTuitionRepository.ProductTuitionExists(billRequest.ProductTuitionId);
                if (!existsRent)
                {
                    throw new HttpRequestException("Não existe esse ProductTuition", null, HttpStatusCode.BadRequest);
                }
            }

            //Se o status for pago, deve ter o modo de pagamento
            if (billForUpdate.Status.ToLower() == BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && (billRequest.PaymentMode == null || billRequest.PayDate == null))
                throw new HttpRequestException("Modo de pagamento e data de pagamento devem ser inseridos para fatura paga", null, HttpStatusCode.BadRequest);

            //Se o status for diferente de pago, o modo de pagamento e a data de pagamento não devem existir
            if (billForUpdate.Status.ToLower() != BillStatus.BillStatusEnum.ElementAt(1)) /*payed*/
            {
                billRequest.PaymentMode = null;
                billRequest.PayDate = null;
            }

            if (billRequest.PaymentMode != null && !PaymentModes.PaymentModesEnum.Contains(billRequest.PaymentMode.ToLower()))
                throw new HttpRequestException("Não existe esse modo de pagamento", null, HttpStatusCode.BadRequest);

            billForUpdate.ProductTuitionId = billRequest.ProductTuitionId;
            billForUpdate.Value = billRequest.Value;
            billForUpdate.Order = billRequest.Order;
            billForUpdate.PayDate = billRequest.PayDate;
            billForUpdate.DueDate = billRequest.DueDate.Value;
            billForUpdate.InvoiceEmittedDate = billRequest.InvoiceEmittedDate;
            billForUpdate.InvoiceId = billRequest.InvoiceId;
            billForUpdate.PaymentMode = billRequest.PaymentMode;
            billForUpdate.Description = billRequest.Description;
            billForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            billForUpdate.UpdatedBy = _email;

            await _billRepository.UpdateBill(billForUpdate);
        }

        public async Task UpdateStatus(string status, string paymentMode, DateTime? payDate, int? InvoiceId, int id)
        {
            var billForUpdate = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (status.ToLower() == BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && (paymentMode == null || payDate == null))
                throw new HttpRequestException("Modo de pagamento e data de pagamento devem ser inseridos para fatura paga", null, HttpStatusCode.BadRequest);

            if (paymentMode != null && !PaymentModes.PaymentModesEnum.Contains(paymentMode.ToLower()))
                throw new HttpRequestException("Não existe esse modo de pagamento", null, HttpStatusCode.BadRequest);

            //Se fatura NÃO paga
            if (status.ToLower() != BillStatus.BillStatusEnum.ElementAt(1) /*payed*/)
            {
                //Se tem um produto nessa fatura
                if (billForUpdate.ProductTuitionId != null)
                {
                    //Se o produto dessa fatura for locação diária ou semanal, modifica o is editable, mas mensal sempre iseditable é true
                    if (billForUpdate.ProductTuition.TimePeriod != ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(2) /*month*/)
                    {
                        billForUpdate.ProductTuition.IsEditable = true;
                        await _productTuitionRepository.UpdateProductTuition(billForUpdate.ProductTuition);
                    }
                }

                paymentMode = null;
                payDate = null;
            }
            //Se for uma fatura que está sendo paga e tem um produto
            else if (status.ToLower() == BillStatus.BillStatusEnum.ElementAt(1) /*payed*/ && billForUpdate.ProductTuitionId != null)
            {
                //Se a fatura a ser editada está indo para paga e se o tipo de locação for diferente de mensal, o producttuition não pode mais ser editado
                if (billForUpdate.ProductTuition.TimePeriod != ProductTuitionPeriodTypes.ProductTuitionPeriodTypesEnum.ElementAt(2) /*month*/)
                {
                    billForUpdate.ProductTuition.IsEditable = false;
                    await _productTuitionRepository.UpdateProductTuition(billForUpdate.ProductTuition);
                }
            }

            billForUpdate.Status = status;
            billForUpdate.PaymentMode = paymentMode;
            billForUpdate.InvoiceId = InvoiceId;
            billForUpdate.PayDate = payDate;
            billForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            billForUpdate.UpdatedBy = _email;

            await _billRepository.UpdateBill(billForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var billForDelete = await _billRepository.GetById(id) ??
                throw new HttpRequestException("Fatura não encontrada", null, HttpStatusCode.NotFound);

            if (billForDelete.InvoiceId != null)
                throw new HttpRequestException("Fatura não pode ser deletada, pois possui Nota Fiscal", null, HttpStatusCode.NotFound);

            billForDelete.Deleted = true;
            billForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            billForDelete.UpdatedBy = _email;

            await _billRepository.UpdateBill(billForDelete);
        }
    }
}