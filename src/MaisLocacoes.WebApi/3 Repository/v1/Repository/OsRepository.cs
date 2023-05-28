using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class OsRepository : IOsRepository
    {
        private readonly PostgreSqlContext _context;

        public OsRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<OsEntity> CreateOs(OsEntity osEntity)
        {
            await _context.Oss.AddAsync(osEntity);
            await _context.SaveChangesAsync();
            return osEntity;
        }

        public async Task<OsEntity> GetById(int id) => await _context.Oss.FirstOrDefaultAsync(o => o.Id == id && o.Deleted == false);

        public async Task<int> UpdateOs(OsEntity osForUpdate)
        {
            _context.Oss.Update(osForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}