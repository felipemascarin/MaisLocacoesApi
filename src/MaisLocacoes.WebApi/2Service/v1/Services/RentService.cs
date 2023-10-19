using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IBillRepository _billRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductTuitionRepository _productTuitionRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentService(IRentRepository rentRepository,
            IBillRepository billRepository,
            IClientRepository clientRepository,
            IProductTuitionRepository productTuitionRepository,
            IAddressService addressService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentRepository = rentRepository;
            _billRepository = billRepository;
            _clientRepository = clientRepository;
            _productTuitionRepository = productTuitionRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<RentResponse> CreateRent(RentRequest rentRequest)
        {
            var client = await _clientRepository.GetById(rentRequest.ClientId) ??
                throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);

            if (client.Status != ClientStatus.ClientStatusEnum.ElementAt(0))
                throw new HttpRequestException("Esse cliente não pode realizar locação", null, HttpStatusCode.BadRequest);

            var addressResponse = await _addressService.CreateAddress(rentRequest.Address);

            var rentEntity = _mapper.Map<RentEntity>(rentRequest);

            rentEntity.AddressId = addressResponse.Id;
            rentEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            rentEntity = await _rentRepository.CreateRent(rentEntity);

            CreateCarriageBill(rentEntity);

            var rentResponse = _mapper.Map<RentResponse>(rentEntity);

            return rentResponse;
        }

        public async Task<GetRentClientResponse> GetById(int id)
        {
            var rentEntity = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            var rentResponse = _mapper.Map<GetRentClientResponse>(rentEntity);

            return rentResponse;
        }

        public async Task<IEnumerable<RentResponse>> GetAllByClientId(int clientId)
        {
            var rentsEntityList = await _rentRepository.GetAllByClientId(clientId);

            var rentsResponseList = _mapper.Map<IEnumerable<RentResponse>>(rentsEntityList);

            return rentsResponseList;
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

        public async Task<bool> UpdateRent(RentRequest rentRequest, int id)
        {
            var rentForUpdate = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            if (rentRequest.ClientId != rentForUpdate.ClientId)
            {
                var client = await _clientRepository.GetById(rentRequest.ClientId) ??
                throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);

                if (client.Status != ClientStatus.ClientStatusEnum.ElementAt(0))
                    throw new HttpRequestException("Esse cliente não pode realizar locação", null, HttpStatusCode.BadRequest);
            }

            rentForUpdate.ClientId = rentRequest.ClientId;
            rentForUpdate.Carriage = rentRequest.Carriage;
            rentForUpdate.Description = rentRequest.Description;
            rentForUpdate.SignedAt = rentRequest.SignedAt;
            rentForUpdate.UrlSignature = rentRequest.UrlSignature;
            rentForUpdate.UpdatedAt = System.DateTime.Now;
            rentForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (!await _addressService.UpdateAddress(rentRequest.Address, rentForUpdate.AddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar a locação", null, HttpStatusCode.InternalServerError);

            if (await _rentRepository.UpdateRent(rentForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> UpdateStatus(string status, int id)
        {
            var rentForUpdate = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            rentForUpdate.Status = status;
            rentForUpdate.UpdatedAt = System.DateTime.Now;
            rentForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _rentRepository.UpdateRent(rentForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var rentForDelete = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            rentForDelete.Deleted = true;
            rentForDelete.UpdatedAt = System.DateTime.Now;
            rentForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _rentRepository.UpdateRent(rentForDelete) > 0) return true;
            else return false;
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
                bill.DueDate = DateTime.Now;
                bill.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

                _billRepository.CreateBill(bill);
            }
        }
    }
}