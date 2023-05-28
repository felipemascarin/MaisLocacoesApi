using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IOsService
    {
        Task<OsResponse> CreateOs(OsRequest osRequest);
        Task<OsResponse> GetById(int id);
        Task<bool> UpdateOs(OsRequest osRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
        Task<bool> DeleteById(int id);
    }
}