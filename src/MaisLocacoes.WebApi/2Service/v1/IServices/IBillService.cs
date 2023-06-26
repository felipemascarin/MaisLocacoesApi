using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using MaisLocacoes.WebApi.Utils.Enums;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<BillResponse> CreateBill(BillRequest billRequest);
        Task<BillResponse> GetById(int id);
        Task<IEnumerable<GetBillProductTypeForRentResponse>> GetByRentId(int rentId);
        Task<IEnumerable<BillResponse>> GetDuedBills();
        Task<bool> UpdateBill(BillRequest billRequest, int id);
        Task<bool> UpdateStatus(string status, string paymentMode, DateTime? payDate, int id);
        Task<bool> DeleteById(int id);
    }
}