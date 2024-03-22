using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace Repository.v1.IRepository
{
    public interface IBillRepository
    {
        Task<BillEntity> CreateBill(BillEntity billEntity);
        Task<BillEntity> GetById(int id);
        Task<IEnumerable<BillEntity>> GetAllDebts(int items, int page);
        Task<BillEntity> GetForTaxInvoice(int id);
        Task<int?> GetTheLastInvoiceId();
        Task<IEnumerable<BillEntity>> GetByRentId(int rentId);
        Task<IEnumerable<BillEntity>> GetByProductTuitionId(int? productTuitionId);
        Task<IEnumerable<BillEntity>> GetDuedBills(int items,int page,int notifyDaysBefore, DateTime todayDate);
        Task<int> UpdateBill(BillEntity billForUpdate);
        Task<int> DeleteBill(BillEntity billForDelete);
    }
}