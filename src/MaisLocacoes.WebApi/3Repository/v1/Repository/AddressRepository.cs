using MaisLocacoes.WebApi.DataBase.Context.Factory;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public AddressRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<AddressEntity> CreateAddress(AddressEntity addressEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Addresses.AddAsync(addressEntity);
            context.SaveChanges();
            return addressEntity;
        }

        public async Task<AddressEntity> GetById(int addressId)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Addresses.Where(a => a.Id == addressId).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAddress(AddressEntity addressForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Addresses.Update(addressForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}