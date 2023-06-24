using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<BillResponse> CreateBill(BillRequest billRequest);
        Task<BillResponse> GetById(int id);
        Task<IEnumerable<BillResponse>> GetByRentId(int rentId);
        Task<bool> UpdateBill(BillRequest billRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> UpdatePaymentMode(string paymentMode, int id);
        Task<bool> DeleteById(int id);
    }
}