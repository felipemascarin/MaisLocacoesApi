using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using Repository.v1.Repository;
using Service.v1.IServices;
using System.Net;

namespace Service.v1.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierService(ISupplierRepository supplierRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _supplierRepository = supplierRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        //public async Task<bool> DeleteById(int id)
        //{
        //    var supplierForDelete = await _supplierRepository.GetById(id) ??
        //        throw new HttpRequestException("Mensalidade não encontrada", null, HttpStatusCode.NotFound);

        //    supplierForDelete.Deleted = true;
        //    supplierForDelete.UpdatedAt = System.DateTime.UtcNow;
        //    supplierForDelete.UpdatedBy = JwtManager.GetEmailByToken(_httpContextAccessor);

        //    if (await _supplierRepository.UpdateSupplier(supplierForDelete) > 0) return true;
        //    else return false;
        //}
    }
}