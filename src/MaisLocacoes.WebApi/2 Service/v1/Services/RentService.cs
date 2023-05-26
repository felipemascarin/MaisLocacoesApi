using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
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

        public async Task<CreateRentResponse> CreateRent(RentRequest rentRequest)
        {
            var existsClient = await _clientRepository.ClientExists(rentRequest.ClientId);
            if (!existsClient)
            {
                throw new HttpRequestException("Não existe esse cliente", null, HttpStatusCode.BadRequest);
            }

            var addressResponse = await _addressService.CreateAddress(rentRequest.Address);

            var rentEntity = _mapper.Map<RentEntity>(rentRequest);

            rentEntity.AddressId = addressResponse.Id;
            rentEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            rentEntity = await _rentRepository.CreateRent(rentEntity);

            var rentResponse = _mapper.Map<CreateRentResponse>(rentEntity);
            rentResponse.Address = addressResponse;

            return rentResponse;
        }

        public async Task<GetRentResponse> GetById(int id)
        {
            var rentEntity = await _rentRepository.GetById(id) ??
                throw new HttpRequestException("Locação não encontrada", null, HttpStatusCode.NotFound);

            var RentAddressResponse = _mapper.Map<GetAddressResponse>(rentEntity.AddressEntity);

            var RentResponse = _mapper.Map<GetRentResponse>(rentEntity);

            RentResponse.Address = RentAddressResponse;

            return RentResponse;
        }
    }
}