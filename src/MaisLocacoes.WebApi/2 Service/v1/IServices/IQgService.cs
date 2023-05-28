using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IQgService
    {
        Task<QgResponse> CreateQg(QgRequest qgRequest);
        Task<QgResponse> GetById(int id);
        Task<bool> UpdateQg(QgRequest qgRequest, int id);
        Task<bool> DeleteById(int id);
    }
}