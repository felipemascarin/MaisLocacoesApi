using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IRentService
    {
        Task<RentResponse> CreateRent(RentRequest rentRequest);
        Task<RentResponse> GetById(int id);
        Task<IEnumerable<RentResponse>> GetRentsByPage(int items, int page, string query);
        Task<bool> UpdateRent(RentRequest rentRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}