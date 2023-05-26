using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IClientService
    {
        Task<CreateClientResponse> CreateClient(ClientRequest clientRequest);
        Task<GetClientResponse> GetById(int id);
        Task<IEnumerable<GetClientResponse>> GetClientsByPage(int items, int page, string query);
        Task<IEnumerable<GetClientForRentResponse>> GetClientsForRent();
        Task<bool> UpdateClient(ClientRequest clientRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}