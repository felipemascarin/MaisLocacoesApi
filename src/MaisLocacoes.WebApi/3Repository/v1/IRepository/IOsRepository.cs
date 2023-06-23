using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IOsRepository
    {
        Task<OsEntity> CreateOs(OsEntity osEntity);
        Task<OsEntity> GetById(int id);
        Task<int> UpdateOs(OsEntity osForUpdate);
    }
}