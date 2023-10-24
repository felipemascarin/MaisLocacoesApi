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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressService _addressService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo _timeZone;
        private readonly string _email;

        public SupplierService(ISupplierRepository supplierRepository,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _timeZone = TZConvert.GetTimeZoneInfo(JwtManager.GetTimeZoneByToken(_httpContextAccessor));
            _email = JwtManager.GetEmailByToken(_httpContextAccessor);
        }

        public async Task<CreateSupplierResponse> CreateSupplier(CreateSupplierRequest supplierRequest)
        {
            var addressResponse = await _addressService.CreateAddress(supplierRequest.Address);

            var supplierEntity = _mapper.Map<SupplierEntity>(supplierRequest);

            supplierEntity.AddressId = addressResponse.Id;
            supplierEntity.CreatedBy = _email;
            supplierEntity.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);

            supplierEntity = await _supplierRepository.CreateSupplier(supplierEntity);

            var supplierResponse = _mapper.Map<CreateSupplierResponse>(supplierEntity);

            return supplierResponse;
        }

        public async Task<GetSupplierByIdResponse> GetSupplierById(int id)
        {
            var supplierEntity = await _supplierRepository.GetById(id) ??
                throw new HttpRequestException("Fornecedor não encontrado", null, HttpStatusCode.NotFound);

            var supplierResponse = _mapper.Map<GetSupplierByIdResponse>(supplierEntity);

            return supplierResponse;
        }

        public async Task<IEnumerable<GetAllSuppliersResponse>> GetAllSuppliers()
        {
            var suppliersEntityList = await _supplierRepository.GetAll();

            var suppliersResponseList = _mapper.Map<IEnumerable<GetAllSuppliersResponse>>(suppliersEntityList);

            return suppliersResponseList;
        }

        public async Task<bool> UpdateSupplier(UpdateSupplierRequest supplierRequest, int id)
        {
            var supplierForUpdate = await _supplierRepository.GetById(id) ??
                throw new HttpRequestException("Fornecedor não encontrado", null, HttpStatusCode.NotFound);

            supplierForUpdate.Name = supplierRequest.Name;
            supplierForUpdate.Cnpj = supplierRequest.Cnpj;
            supplierForUpdate.Email = supplierRequest.Email;
            supplierForUpdate.Tel = supplierRequest.Tel;
            supplierForUpdate.Cel = supplierRequest.Cel;
            supplierForUpdate.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            supplierForUpdate.UpdatedBy = _email;

            if (!await _addressService.UpdateAddress(supplierRequest.Address, supplierForUpdate.AddressEntity.Id))
                throw new HttpRequestException("Não foi possível salvar endereço antes de salvar o fornecedor", null, HttpStatusCode.InternalServerError);

            if (await _supplierRepository.UpdateSupplier(supplierForUpdate) > 0) return true;
            else return false;
        }

        public async Task<bool> DeleteById(int id)
        {
            var supplierForDelete = await _supplierRepository.GetById(id) ??
                throw new HttpRequestException("Fornecedor não encontrado", null, HttpStatusCode.NotFound);

            supplierForDelete.Deleted = true;
            supplierForDelete.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, _timeZone);
            supplierForDelete.UpdatedBy = _email;

            if (await _supplierRepository.UpdateSupplier(supplierForDelete) > 0) return true;
            else return false;
        }
    }
}