using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IQgService
    {
        Task<CreateQgResponse> CreateQg(CreateQgRequest qgRequest);
        Task<GetQgByIdResponse> GetQgById(int id);
        Task<IEnumerable<GetAllQgsResponse>> GetAllQgs();
        Task<bool> UpdateQg(UpdateQgRequest qgRequest, int id);
        Task<bool> DeleteById(int id);
    }
}