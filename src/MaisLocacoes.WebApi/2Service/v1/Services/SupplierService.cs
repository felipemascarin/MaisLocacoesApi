using AutoMapper;
using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.Entity;
using Repository.v1.IRepository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressService _addressService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository,
            IAddressService addressService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _addressService = addressService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<SupplierResponse> CreateSupplier(SupplierRequest supplierRequest)
        {
            var addressResponse = await _addressService.CreateAddress(supplierRequest.Address);

            var supplierEntity = _mapper.Map<SupplierEntity>(supplierRequest);

            supplierEntity.AddressId = addressResponse.Id;
            supplierEntity.CreatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            supplierEntity = await _supplierRepository.CreateSupplier(supplierEntity);

            var supplierResponse = _mapper.Map<SupplierResponse>(supplierEntity);

            return supplierResponse;
        }

        public async Task<SupplierResponse> GetById(int id)
        {
            var supplierEntity = await _supplierRepository.GetById(id) ??
                throw new HttpRequestException("Fornecedor não encontrado", null, HttpStatusCode.NotFound);

            var supplierResponse = _mapper.Map<SupplierResponse>(supplierEntity);

            return supplierResponse;
        }

        public async Task<IEnumerable<SupplierResponse>> GetAll()
        {
            var suppliersEntityList = await _supplierRepository.GetAll();

            var suppliersResponseList = _mapper.Map<IEnumerable<SupplierResponse>>(suppliersEntityList);

            return suppliersResponseList;
        }

        public async Task<bool> UpdateSupplier(SupplierRequest supplierRequest, int id)
        {
            var supplierForUpdate = await _supplierRepository.GetById(id) ??
                throw new HttpRequestException("Fornecedor não encontrado", null, HttpStatusCode.NotFound);

            supplierForUpdate.Name = supplierRequest.Name;
            supplierForUpdate.Cnpj = supplierRequest.Cnpj;
            supplierForUpdate.Email = supplierRequest.Email;
            supplierForUpdate.Tel = supplierRequest.Tel;
            supplierForUpdate.Cel = supplierRequest.Cel;
            supplierForUpdate.UpdatedAt = System.DateTime.Now;
            supplierForUpdate.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

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
            supplierForDelete.UpdatedAt = System.DateTime.Now;
            supplierForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

            if (await _supplierRepository.UpdateSupplier(supplierForDelete) > 0) return true;
            else return false;
        }
    }
}