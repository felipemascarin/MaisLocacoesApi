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
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentService(IRentRepository rentRepository,
            IClientRepository clientRepository,
            IAddressService addressService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentRepository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _clientRepository = clientRepository;
            _addressService = addressService;
        }

        public async Task<RentResponse> CreateRent(RentRequest rentRequest)
        {
            var existsClient = await _clientRepository.ClientExists(rentRequest.ClientId);
            if (!existsClient)
            {
                throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);
            }

            var addressResponse = await _addressService.CreateAddress(rentRequest.Address);

            var rentEntity = _mapper.Map<RentEntity>(rentRequest);

            rentEntity.AddressId = addressResponse.Id;
            rentEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            rentEntity = await _rentRepository.CreateRent(rentEntity);

            var rentResponse = _mapper.Map<RentResponse>(rentEntity);
            rentResponse.Address = addressResponse;

            return rentResponse;
        }

        public async Task<RentResponse> GetById(int id)
        {
            var rentEntity = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            var RentAddressResponse = _mapper.Map<AddressResponse>(rentEntity.AddressEntity);

            var RentResponse = _mapper.Map<RentResponse>(rentEntity);

            RentResponse.Address = RentAddressResponse;

            return RentResponse;
        }

        public async Task<IEnumerable<RentResponse>> GetRentsByPage(int items, int page, string query)
        {
            if (items <= 0 || page <= 0)
                throw new HttpRequestException("Informe o valor da página e a quantidade de itens corretamente", null, HttpStatusCode.BadRequest);

            var rentsEntityList = await _rentRepository.GetRentsByPage(items, page, query);

            var rentsEntityListLenght = rentsEntityList.ToList().Count;

            var rentsResponseList = _mapper.Map<IEnumerable<RentResponse>>(rentsEntityList);

            for (int i = 0; i < rentsEntityListLenght; i++)
            {
                rentsResponseList.ElementAt(i).Address = _mapper.Map<AddressResponse>(rentsEntityList.ElementAt(i).AddressEntity);
            }

            return rentsResponseList;
        }

        public async Task<bool> UpdateRent(RentRequest rentRequest, int id)
        {
            var rentForUpdate = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            if (rentRequest.ClientId != rentForUpdate.ClientId)
            {
                var existsClient = await _clientRepository.ClientExists(rentRequest.ClientId);
                if (!existsClient)
                    throw new HttpRequestException("O cliente informado não existe", null, HttpStatusCode.BadRequest);
            }

            rentForUpdate.ClientId = rentRequest.ClientId;
            rentForUpdate.Carriage = rentRequest.Carriage;
            rentForUpdate.UpdatedAt = System.DateTime.UtcNow;
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
            rentForUpdate.UpdatedAt = System.DateTime.UtcNow;
            rentForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _rentRepository.UpdateRent(rentForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var rentForDelete = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            rentForDelete.Deleted = true;
            rentForDelete.UpdatedAt = System.DateTime.UtcNow;
            rentForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _rentRepository.UpdateRent(rentForDelete) > 0) return true;
            else return false;
        }
    }
}