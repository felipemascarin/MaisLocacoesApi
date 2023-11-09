using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Contract;

namespace MaisLocacoes.WebApi._2Service.v1.IServices
{
    public interface IContractService
    {
        Task<CreateContractResponse> CreateContract(CreateContractRequest contractRequest);
        Task<GetContractByIdResponse> GetContractById(int id);
        Task<IEnumerable<GetAllContractsResponse>> GetAllContracts();
        Task<GetContractInfoByRentIdResponse> GetContractInfoByRentId(int rentId);
        Task UpdateContract(UpdateContractRequest contractRequest, int id);
        Task DeleteById(int id);
    }
}
