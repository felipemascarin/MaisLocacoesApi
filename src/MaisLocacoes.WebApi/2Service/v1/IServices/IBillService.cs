using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Bill;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<CreateBillResponse> CreateBill(CreateBillRequest billRequest);
        Task<GetBillByIdResponse> GetBillById(int id);
        Task<GetBillForTaxInvoiceResponse> GetBillForTaxInvoice(int billId);
        Task<IEnumerable<GetBillByRentIdResponse>> GetBillByRentId(int rentId);
        Task<IEnumerable<GetDuedsBillsResponse>> GetDuedBills();
        Task<IEnumerable<GetAllBillsDebtsResponse>> GetAllBillsDebts();
        Task UpdateBill(UpdateBillRequest billRequest, int id);
        Task UpdateStatus(string status, string paymentMode, DateTime? payDate, int? invoiceId, int id);
        Task DeleteById(int id);
    }
}