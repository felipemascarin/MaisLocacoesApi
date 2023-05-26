using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;

namespace Service.v1.IServices
{
    public interface IBillService
    {
        Task<CreateBillResponse> CreateBill(BillRequest billRequest);
    }
}