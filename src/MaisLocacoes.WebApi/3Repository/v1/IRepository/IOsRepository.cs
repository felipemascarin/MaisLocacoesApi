using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IOsRepository
    {
        Task<OsEntity> CreateOs(OsEntity osEntity);
        Task<OsEntity> GetById(int id);
        Task<IEnumerable<OsEntity>> GetAllByStatus(string status);
        Task<OsEntity> GetByProductTuitionId(int productTuitionId, string type);
        Task<OsEntity> GetByProductTuitionIdForCreate(int productTuitionId, string type);
        Task<int> UpdateOs(OsEntity osForUpdate);
    }
}