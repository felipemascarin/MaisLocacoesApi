using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class RentedPlaceRepository : IRentedPlaceRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public RentedPlaceRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
            using var _context = _contextFactory.CreateContext();
        }

        public async Task<RentedPlaceEntity> CreateRentedPlace(RentedPlaceEntity rentedPlaceEntity)
        {
            await _context.RentedPlaces.AddAsync(rentedPlaceEntity);
            _context.SaveChanges();
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