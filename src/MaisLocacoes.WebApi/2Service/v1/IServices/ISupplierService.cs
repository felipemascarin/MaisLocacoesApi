using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface ISupplierService
    {
        Task<CreateSupplierResponse> CreateSupplier(CreateSupplierRequest supplierRequest);
        Task<GetSupplierByIdResponse> GetSupplierById(int id);
        Task<IEnumerable<GetAllSuppliersResponse>> GetAllSuppliers();
        Task UpdateSupplier(UpdateSupplierRequest supplierRequest, int id);
        Task DeleteById(int id);
    }
}