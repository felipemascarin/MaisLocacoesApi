using MaisLocacoes.WebApi.Context;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class QgRepository : IQgRepository
    {
        private readonly PostgreSqlContext _context;

        public QgRepository(PostgreSqlContext context)
        {
            _context = context;
        }

        public async Task<QgEntity> CreateQg(QgEntity qgEntity)
        {
            await _context.Qgs.AddAsync(qgEntity);
            _context.SaveChanges();
            return qgEntity;
        }

        public async Task<QgEntity> GetById(int id) => await _context.Qgs.FirstOrDefaultAsync(q => q.Id == id && q.Deleted == false);
        
        public async Task<bool> QgExists(int id) => await _context.Qgs.AnyAsync(q => q.Id == id && q.Deleted == false);

        public async Task<int> UpdateQg(QgEntity qgForUpdate)
        {
            _context.Qgs.Update(qgForUpdate);
            return await _context.SaveChangesAsync();
        }
    }
}