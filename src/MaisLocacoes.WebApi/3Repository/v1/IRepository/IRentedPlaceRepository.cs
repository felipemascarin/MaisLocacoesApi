using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IRentedPlaceRepository
    {
        Task<RentedPlaceEntity> CreateRentedPlace(RentedPlaceEntity rentedPlaceEntity);
        Task<RentedPlaceEntity> GetById(int id);
        Task<IEnumerable<RentedPlaceEntity>> GetTheLastRentedPlaceByIds(List<int> ids);
        Task<int> UpdateRentedPlace(RentedPlaceEntity rentedPlaceForUpdate);
    }
}