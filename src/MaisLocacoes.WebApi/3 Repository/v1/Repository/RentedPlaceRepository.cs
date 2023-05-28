using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class RentedPlaceRepository : IRentedPlaceRepository
    {
        private readonly PostgreSqlContext _context;

        public RentedPlaceRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<RentedPlaceEntity> CreateRentedPlace(RentedPlaceEntity rentedPlaceEntity)
        {
            await _context.RentedPlaces.AddAsync(rentedPlaceEntity);
            await _context.SaveChangesAsync();
            return rentedPlaceEntity;
        }

        public async Task<RentedPlaceEntity> GetById(int id) => await _context.RentedPlaces.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<int> UpdateRentedPlace(RentedPlaceEntity rentedPlaceForUpdate)
        {
            _context.RentedPlaces.Update(rentedPlaceForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}