using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Request.Create.UserSchema;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Create;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IRentService
    {
        Task<CreateRentResponse> CreateRent(RentRequest rentRequest);
        Task<GetRentResponse> GetById(int id);
        Task<bool> UpdateRent(RentRequest rentRequest, int id);
        Task<bool> UpdateStatus(string status, int id);
    }
}