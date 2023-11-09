using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Rent;

namespace Service.v1.IServices
{
    public interface IRentService
    {
        Task<CreateRentResponse> CreateRent(CreateRentRequest rentRequest);
        Task<GetRentByIdResponse> GetRentById(int id);
        Task<IEnumerable<GetAllRentsByClientIdResponse>> GetAllRentsByClientId(int clientId);
        Task<IEnumerable<GetRentByPageResponse>> GetRentsByPage(int items, int page, string query, string status);
        Task UpdateRent(UpdateRentRequest rentRequest, int id);
        Task UpdateStatus(string status, int id);
        Task DeleteById(int id);
    }
}