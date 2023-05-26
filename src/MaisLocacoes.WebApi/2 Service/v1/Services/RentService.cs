using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentService(IRentRepository rentRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _rentRepository = rentRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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