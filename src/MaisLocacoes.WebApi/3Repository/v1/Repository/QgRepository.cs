using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class QgRepository : IQgRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory; 

        public QgRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory; 
            using var _context = _contextFactory.CreateContext();
        }

        public async Task<QgEntity> CreateQg(QgEntity qgEntity)
        {
            await _context.Qgs.AddAsync(qgEntity);
            _context.SaveChanges();
            return qgEntity;
        }

        public async Task<QgEntity> GetById(int id) => await _context.Qgs.FirstOrDefaultAsync(q => q.Id == id && q.Deleted == false);

        public async Task<IEnumerable<QgEntity>> GetAll() => await _context.Qgs.Where(q => q.Deleted == false).ToListAsync();
        
        public async Task<bool> QgExists(int id) => await _context.Qgs.AnyAsync(q => q.Id == id && q.Deleted == false);

        public async Task<int> UpdateQg(QgEntity qgForUpdate)
        {
            _context.Qgs.Update(qgForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}