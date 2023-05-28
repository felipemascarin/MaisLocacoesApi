using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IQgRepository
    {
        Task<QgEntity> CreateQg(QgEntity qgEntity);
        Task<QgEntity> GetById(int id);
        Task<int> UpdateQg(QgEntity qgForUpdate);
    }
}