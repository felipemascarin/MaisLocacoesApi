using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi._3Repository.v1.IRepository;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi._3Repository.v1.Repository
{
    public class OsPictureRepository : IOsPictureRepository
    {
        private readonly PostgreSqlContextFactory _contextFactory;

        public OsPictureRepository(PostgreSqlContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<OsPictureEntity>> CreateOsPictures(List<OsPictureEntity> osPictureEntities)
        {
            using var context = _contextFactory.CreateContext();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            foreach (var osPictureEntity in osPictureEntities)
            {
                // Define o estado da entidade como "Adicionado"
                context.Entry(osPictureEntity).State = EntityState.Added;
            }

            await context.SaveChangesAsync();
            return osPictureEntities;
        }
    }
}
