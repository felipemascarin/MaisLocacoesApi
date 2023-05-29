using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IClientService
    {
        Task<ClientResponse> CreateClient(ClientRequest clientRequest);
        Task<ClientResponse> GetById(int id);
        Task<ClientResponse> GetByCpf(string cpf);
        Task<ClientResponse> GetByCnpj(string cnpj);
        Task<IEnumerable<ClientResponse>> GetClientsByPage(int items, int page, string query);
        Task<IEnumerable<GetClientForRentResponse>> GetClientsForRent();
        Task<bool> UpdateClient(ClientRequest clientRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}