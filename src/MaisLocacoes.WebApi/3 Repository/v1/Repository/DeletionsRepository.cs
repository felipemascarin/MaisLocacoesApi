using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity;
using MaisLocacoes.WebApi._3_Repository.v1.DeletedEntity.UserSchema;
using MaisLocacoes.WebApi._3_Repository.v1.IRepository;
using MaisLocacoes.WebApi.Context;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi._3_Repository.v1.Repository
{
    public class DeletionsRepository : IDeletionsRepository
    {
        private readonly PostgreSqlContext _context;

        public DeletionsRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<ClientsDeletions> CreateClientsDeletions(ClientsDeletions clientForDelete)
        {
            await _context.ClientsDeletions.AddAsync(clientForDelete);
            await _context.SaveChangesAsync();
            return clientForDelete;
        }

        public async Task<int> DeleteClient(ClientEntity clientForDelete)
        {
            _context.Clients.Remove(clientForDelete);
            return await _context.SaveChangesAsync();
        }

        public async Task<UsersDeletions> CreateUsersDeletions(UsersDeletions userForDelete)
        {
            await _context.UsersDeletions.AddAsync(userForDelete);
            await _context.SaveChangesAsync();
            return userForDelete;
        }

        public async Task<int> DeleteUser(UserEntity userForDelete)
        {
            _context.Users.Remove(userForDelete);
            return await _context.SaveChangesAsync();
        }

        public async Task<BillsDeletions> CreateBillsDeletions(BillsDeletions billForDelete)
        {
            await _context.BillsDeletions.AddAsync(billForDelete);
            await _context.SaveChangesAsync();
            return billForDelete;
        }

        public async Task<int> DeleteBill(BillEntity billForDelete)
        {
            _context.Bills.Remove(billForDelete);
            return await _context.SaveChangesAsync();
        }
    }
}