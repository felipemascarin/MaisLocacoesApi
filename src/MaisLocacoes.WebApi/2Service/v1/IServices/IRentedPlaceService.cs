using MaisLocacoes.WebApi.Domain.Models.v1.Request;
using MaisLocacoes.WebApi.Domain.Models.v1.Response;

namespace Service.v1.IServices
{
    public interface IRentedPlaceService
    {
        Task<RentedPlaceResponse> CreateRentedPlace(RentedPlaceRequest rentedPlaceRequest);
        Task<RentedPlaceResponse> GetById(int id);
        Task<bool> UpdateRentedPlace(RentedPlaceRequest rentedPlaceRequest, int id);
    }
}