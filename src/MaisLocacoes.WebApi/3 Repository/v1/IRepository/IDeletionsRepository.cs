using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi._3_Repository.v1.IRepository
{
    public interface IDeletionsRepository
    {
        Task<ClientsDeletions> CreateClientsDeletions(ClientsDeletions clientForDelete);
        Task<int> DeleteClient(ClientEntity clientForDelete);
    }
}
