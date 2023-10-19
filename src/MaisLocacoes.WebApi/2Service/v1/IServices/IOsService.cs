using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IOsService
    {
        Task<CreateOsResponse> CreateOs(CreateOsRequest osRequest);
        Task<bool> StartOs(int id);
        Task<bool> ReturnOs(int id);
        Task<bool> CancelOs(int id);
        Task<bool> FinishOs(int id, FinishOsRequest closeOsRequest);
        Task<GetOsByIdResponse> GetOsById(int id);
        Task<IEnumerable<GetAllOsByStatusResponse>> GetAllOsByStatus(string status);
        Task<bool> UpdateOs(UpdateOsRequest osRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}