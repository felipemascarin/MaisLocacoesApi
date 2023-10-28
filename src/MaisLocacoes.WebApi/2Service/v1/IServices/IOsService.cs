using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IOsService
    {
        Task<CreateOsResponse> CreateOs(CreateOsRequest osRequest);
        Task StartOs(int id);
        Task ReturnOs(int id);
        Task CancelOs(int id);
        Task FinishOs(int id, FinishOsRequest closeOsRequest);
        Task<GetOsByIdResponse> GetOsById(int id);
        Task<IEnumerable<GetAllOsByStatusResponse>> GetAllOsByStatus(string status);
        Task UpdateOs(UpdateOsRequest osRequest, int id);
        Task UpdateStatus(string status, int id);
        Task DeleteById(int id);
    }
}