using AutoMapper;
using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;
using TimeZoneConverter;
using static MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent.GetAllRentsByClientIdResponse;

namespace Service.v1.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IBillRepository _billRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public RentService(IRentRepository rentRepository,
            IBillRepository billRepository,
            IClientRepository clientRepository,
            IProductTuitionRepository productTuitionRepository,
            IContractRepository contractRepository,
            IAddressService addressService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentRepository = rentRepository;
            _billRepository = billRepository;
            _clientRepository = clientRepository;
            _productTuitionRepository = productTuitionRepository;
            _contractRepository = contractRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateRentResponse> CreateRent(CreateRentRequest rentRequest)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            rentRequest = TimeZoneConverter<CreateRentRequest>.ConvertToTimeZoneLocal(rentRequest, _timeZone);

            var client = await _clientRepository.GetById(rentRequest.ClientId.Value) ??
                throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);

            if (client.Status != ClientStatus.ClientStatusEnum.ElementAt(0))
                throw new HttpRequestException("Esse cliente não pode realizar locação", null, HttpStatusCode.BadRequest);

            var addressResponse = await _addressService.CreateAddress(rentRequest.Address);
            var addressEntity = _mapper.Map<AddressEntity>(addressResponse);

            var rentEntity = _mapper.Map<RentEntity>(rentRequest);

            rentEntity.AddressId = addressResponse.Id;
            rentEntity.Address = addressEntity;
            rentEntity.CreatedBy = _email;
            rentEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            rentEntity = await _rentRepository.CreateRent(rentEntity);

            var contractEntity = new ContractEntity()
            {
                GuidId = Guid.NewGuid(),
                RentId = rentEntity.Id,
                Version = 1,
                UrlSignature = null,
                SignedAt = null,
                UpdatedAt = null,
                UpdatedBy = null,
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone),
                CreatedBy = _email
            };

            await _contractRepository.CreateContract(contractEntity);

            CreateCarriageBill(rentEntity);

            var rentResponse = _mapper.Map<CreateRentResponse>(rentEntity);

            return rentResponse;
        }

        public async Task<GetRentByIdResponse> GetRentById(int id)
        {
            var rentEntity = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            var rentResponse = _mapper.Map<GetRentByIdResponse>(rentEntity);

            return rentResponse;
        }

        public async Task<GetAllRentsByClientIdResponse> GetAllRentsByClientId(int clientId)
        {
            var rentsEntityList = (await _rentRepository.GetAllByClientId(clientId)).ToList();
            rentsEntityList.RemoveAll(rent => rent.ProductTuitions.Count == 0);

            var clientRentsResponse = new GetAllRentsByClientIdResponse
            {
                ClientRentsResponse = _mapper.Map<List<ResumedClientRentDto>>(rentsEntityList),
                TotalBilledValue = 0
            };

            foreach (var rentDto in clientRentsResponse.ClientRentsResponse)
            {
                var bills = rentsEntityList.FirstOrDefault(r => r.Id == rentDto.Id).Bills;

                foreach (var bill in bills)
                {
                    if (bill.Status == BillStatus.BillStatusEnum.ElementAt(1)/*payed*/ && bill.ProductTuitionId != null)
                    {
                        clientRentsResponse.TotalBilledValue += bill.Value;
                        rentDto.BilledValue += bill.Value;
                    }
                }

                bills.Clear();

                foreach (var rentEntity in rentsEntityList)
                {
                    if (rentEntity.Id == rentDto.Id && rentEntity.ProductTuitions.Count > 0)
                    {
                        rentDto.InitialDate = rentEntity.ProductTuitions.OrderBy(x => x.InitialDateTime).FirstOrDefault().InitialDateTime;
                        rentDto.FinalDate = rentEntity.ProductTuitions.OrderByDescending(x => x.FinalDateTime).FirstOrDefault().FinalDateTime;
                    }
                }
            }

            clientRentsResponse.ClientRentsResponse.OrderByDescending(r => r.Id);

            return clientRentsResponse;
        }

        public async Task<IEnumerable<GetRentByPageResponse>> GetRentsByPage(int items, int page, string query, string status)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var rentsEntityList = await _rentRepository.GetRentsByPage(items, page, query, status);

            var rentsResponseList = _mapper.Map<IEnumerable<GetRentByPageResponse>>(rentsEntityList);

            foreach (var rent in rentsResponseList)
            {
                var productTuitions = (await _productTuitionRepository.GetAllByRentId(rent.Id)).ToList();

                foreach (var productTuition in productTuitions)
                {
                    if (productTuition.ProductCode != null)
                    {
                        rent.ProductCodes.Add(productTuition.ProductCode);
                    }

                    productTuitions.OrderBy(p => p.InitialDateTime);
                    rent.InitialDate = productTuitions.FirstOrDefault().InitialDateTime;

                    productTuitions.OrderByDescending(p => p.FinalDateTime);
                    rent.FinalDate = productTuitions.FirstOrDefault().FinalDateTime;
                }

                productTuitions.Clear();
            }

            var rentsResponseListReturn = rentsResponseList.OrderByDescending(r => r.Id);

            return rentsResponseListReturn;
        }

        public async Task UpdateRent(UpdateRentRequest rentRequest, int id)
        {
            //Converte todas as propridades que forem data (utc) para o timezone da empresa
            rentRequest = TimeZoneConverter<UpdateRentRequest>.ConvertToTimeZoneLocal(rentRequest, _timeZone);

            var rentForUpdate = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            if (rentRequest.ClientId != rentForUpdate.ClientId)
            {
                var client = await _clientRepository.GetById(rentRequest.ClientId.Value) ??
                throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);

                if (client.Status != ClientStatus.ClientStatusEnum.ElementAt(0))
                    throw new HttpRequestException("Esse cliente não pode realizar locação", null, HttpStatusCode.BadRequest);
            }

            rentForUpdate.ClientId = rentRequest.ClientId.Value;
            rentForUpdate.Carriage = rentRequest.Carriage;
            rentForUpdate.Description = rentRequest.Description;
            rentForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            rentForUpdate.UpdatedBy = _email;

            await _addressService.UpdateAddress(rentRequest.Address, rentForUpdate.Address.Id);

            await _rentRepository.UpdateRent(rentForUpdate);
        }

        public async Task UpdateStatus(string status, int id)
        {
            var rentForUpdate = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            rentForUpdate.Status = status;
            rentForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            rentForUpdate.UpdatedBy = _email;

            await _rentRepository.UpdateRent(rentForUpdate);
        }

        public async Task DeleteById(int id)
        {
            var rentForDelete = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            rentForDelete.Deleted = true;
            rentForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            rentForDelete.UpdatedBy = _email;

            await _rentRepository.UpdateRent(rentForDelete);
        }

        public void CreateCarriageBill(RentEntity rentEntity)
        {
            if (rentEntity.Carriage != null && rentEntity.Carriage > 0)
            {
                var bill = new BillEntity();

                bill.RentId = rentEntity.Id;
                bill.ProductTuitionId = null;
                bill.Value = rentEntity.Carriage.Value;
                bill.Status = BillStatus.BillStatusEnum.ElementAt(2);
                bill.PaymentMode = null;
                bill.Description = "Fatura de frete";
                bill.DueDate = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
                bill.CreatedBy = _email;

                _billRepository.CreateBill(bill);
            }
        }
    }
}