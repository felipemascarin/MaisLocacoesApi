using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;
using MaisLocacoes.WebApi.Domain.Models.v1.Response.Get;

namespace Service.v1.IServices
{
    public interface IRentedPlaceService
    {
        Task<CreateRentedPlaceResponse> CreateRentedPlace(CreateRentedPlaceRequest rentedPlaceRequest);
        Task<GetRentedPlaceByIdResponse> GetRentedPlaceById(int id);
        Task UpdateRentedPlace(UpdateRentedPlaceRequest rentedPlaceRequest, int id);
    }
}