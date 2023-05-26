using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;
using Repository.v1.IRepository.UserSchema;

namespace Repository.v1.Repository.UserSchema
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSqlContext _context;

        public UserRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateUser(UserEntity userEntity)
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        public async Task<bool> UserHasToken(string token, string email) => await _context.Users.Where(u => u.Email == email).AnyAsync(u => u.LastToken == token);

        public async Task<UserEntity> GetByEmail(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<UserEntity> GetByCpf(string cpf) => await _context.Users.FirstOrDefaultAsync(u => u.Cpf == cpf);

        public async Task<bool> UserExists(string email, string cpf) => await _context.Users.AnyAsync(u => u.Email == email || u.Cpf == cpf);

        public async Task<int> UpdateUser(UserEntity userForUpdate)
        {
            _context.Users.Update(userForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}