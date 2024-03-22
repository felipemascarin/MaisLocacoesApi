using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;
using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IClientRepository
    {
        Task<ClientEntity> CreateClient(ClientEntity clientEntity);
        Task<ClientEntity> GetById(int id);
        Task<ClientEntity> GetByIdDetails(int id);
        Task<ClientEntity> GetByCpf(string cpf);
        Task<ClientEntity> GetByCnpj(string cnpj);
        Task<bool> ClientExists(int id);
        Task<IEnumerable<ClientEntity>> GetClientsByPage(int items, int page, string query);
        Task<IEnumerable<GetClientForRentDtoResponse>> GetClientsForRent();
        Task<int> UpdateClient(ClientEntity clientForUpdate);
        Task<int> DeleteClient(ClientEntity clientForDelete);
    }
}