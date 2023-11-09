using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Qg;

namespace Service.v1.IServices
{
    public interface IQgService
    {
        Task<CreateQgResponse> CreateQg(CreateQgRequest qgRequest);
        Task<GetQgByIdResponse> GetQgById(int id);
        Task<IEnumerable<GetAllQgsResponse>> GetAllQgs();
        Task UpdateQg(UpdateQgRequest qgRequest, int id);
        Task DeleteById(int id);
    }
}