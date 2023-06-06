using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IRentService
    {
        Task<RentResponse> CreateRent(RentRequest rentRequest);
        Task<RentResponse> GetById(int id);
        Task<IEnumerable<RentResponse>> GetAllByClientId(int clientId);
        Task<IEnumerable<GetRentClientResponse>> GetRentsByPage(int items, int page, string query, string status);
        Task<bool> UpdateRent(RentRequest rentRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}