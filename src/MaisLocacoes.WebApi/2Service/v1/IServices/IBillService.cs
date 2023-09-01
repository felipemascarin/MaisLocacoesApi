using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<BillResponse> CreateBill(BillRequest billRequest);
        Task<BillResponse> GetById(int id);
        Task<GetBillForTaxInvoiceResponse> GetForTaxInvoice(int billId);
        Task<IEnumerable<GetBillProductTypeForRentResponse>> GetByRentId(int rentId);
        Task<IEnumerable<GetDuedsBillsResponse>> GetDuedBills();
        Task<IEnumerable<GetDebtsBillsResponse>> GetAllDebts();
        Task<bool> UpdateBill(BillRequest billRequest, int id);
        Task<bool> UpdateStatus(string status, string paymentMode, DateTime? payDate, int? nfIdFireBase, int id);
        Task<bool> DeleteById(int id);
    }
}