using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly PostgreSqlContext _context;

        public AddressRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<AddressEntity> CreateAddress(AddressEntity addressEntity)
        {
                await _context.Addresses.AddAsync(addressEntity);
                await _context.SaveChangesAsync();
                return addressEntity;
        }

        public async Task<AddressEntity> GetById(int addressId) 
            => await _context.Addresses.Where(a => a.Id == addressId).FirstOrDefaultAsync();

        public async Task<int> UpdateAddress(AddressEntity addressForUpdate)
        {
            _context.Addresses.Update(addressForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}