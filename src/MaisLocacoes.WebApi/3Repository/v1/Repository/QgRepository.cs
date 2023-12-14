using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context.Factory;
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
        }

        public async Task<QgEntity> CreateQg(QgEntity qgEntity)
        {
            using var context = _contextFactory.CreateContext();
            await context.Qgs.AddAsync(qgEntity);
            context.SaveChanges();
            return qgEntity;
        }

        public async Task<QgEntity> GetById(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Qgs.FirstOrDefaultAsync(q => q.Id == id && q.Deleted == false);
        }

        public async Task<IEnumerable<QgEntity>> GetAll()
        {
            using var context = _contextFactory.CreateContext();
            return await context.Qgs.Where(q => q.Deleted == false).ToListAsync();
        }

        public async Task<bool> QgExists(int id)
        {
            using var context = _contextFactory.CreateContext();
            return await context.Qgs.AnyAsync(q => q.Id == id && q.Deleted == false);
        }

        public async Task<int> UpdateQg(QgEntity qgForUpdate)
        {
            using var context = _contextFactory.CreateContext();
            context.Qgs.Update(qgForUpdate);
            return await context.SaveChangesAsync();
        }
    }
}