using MaisLocacoes.WebApi._3Repository.v1.Entity;

namespace MaisLocacoes.WebApi._3Repository.v1.IRepository
{
    public interface IOsPictureRepository
    {
        Task<List<OsPictureEntity>> CreateOsPictures(List<OsPictureEntity> osPictureEntities);
    }
}
