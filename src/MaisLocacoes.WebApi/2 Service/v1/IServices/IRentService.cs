using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IRentService
    {
        Task<CreateRentResponse> CreateRent(RentRequest rentRequest);
        Task<GetRentResponse> GetById(int id);
    }
}