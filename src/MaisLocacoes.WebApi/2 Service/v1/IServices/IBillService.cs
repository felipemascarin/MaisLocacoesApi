using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<CreateBillResponse> CreateBill(BillRequest billRequest);
        Task<GetBillResponse> GetById(int id);
        Task<bool> UpdateBill(BillRequest billRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}