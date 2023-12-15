using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
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
        }

        public async Task<RentedPlaceEntity> CreateRentedPlace(RentedPlaceEntity rentedPlaceEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Set<RentedPlaceEntity>().AddAsync(rentedPlaceEntity);
            context.SaveChanges();
            return rentedPlaceEntity;
        }

        public async Task<RentedPlaceEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Set<RentedPlaceEntity>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> UpdateRentedPlace(RentedPlaceEntity rentedPlaceForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Set<RentedPlaceEntity>().Update(rentedPlaceForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}