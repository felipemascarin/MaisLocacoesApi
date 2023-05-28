using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface ISupplierService
    {
        Task<SupplierResponse> CreateSupplier(SupplierRequest supplierRequest);
        Task<SupplierResponse> GetById(int id);
        Task<bool> UpdateSupplier(SupplierRequest supplierRequest, int id);
        Task<bool> DeleteById(int id);
    }
}