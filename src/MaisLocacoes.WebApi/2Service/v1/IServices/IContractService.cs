using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace MaisLocacoes.WebApi._2Service.v1.IServices
{
    public interface IContractService
    {
        Task<CreateContractResponse> CreateContract(CreateContractRequest contractRequest);
        Task<GetContractByIdResponse> GetContractById(int id);
        Task<IEnumerable<GetAllContractsResponse>> GetAllContracts();
        Task UpdateContract(UpdateContractRequest contractRequest, int id);
        Task DeleteById(int id);
    }
}
