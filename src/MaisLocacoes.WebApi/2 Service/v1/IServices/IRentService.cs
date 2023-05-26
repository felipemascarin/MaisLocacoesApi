using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IRentService
    {

        Task<GetRentResponse> GetById(int id);
    }
}