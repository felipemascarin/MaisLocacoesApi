using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IClientService
    {
        Task<CreateClientResponse> CreateClient(CreateClientRequest clientRequest);
        Task<GetClientByIdResponse> GetClientById(int id);
        Task<GetClientByIdDetailsResponse> GetClientByIdDetails(int id);
        Task<GetClientByCpfResponse> GetClientByCpf(string cpf);
        Task<GetClientByCnpjResponse> GetClientByCnpj(string cnpj);
        Task<IEnumerable<GetClientsByPageResponse>> GetClientsByPage(int items, int page, string query);
        Task<IEnumerable<GetClientsForRentResponse>> GetClientsForRent();
        Task<bool> UpdateClient(UpdateClientRequest clientRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}