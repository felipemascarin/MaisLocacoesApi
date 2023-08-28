using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Custom;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IOsService
    {
        Task<OsResponse> CreateOs(OsRequest osRequest);
        Task<bool> StartOs(int id);
        Task<bool> ReturnOs(int id);
        Task<bool> CancelOs(int id);
        Task<bool> FinishOs(int id, CloseOsRequest closeOsRequest);
        Task<OsResponse> GetById(int id);
        Task<IEnumerable<OsResponse>> GetAllByStatus(string status);
        Task<bool> UpdateOs(OsRequest osRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}