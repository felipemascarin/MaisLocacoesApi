using Repository.v1.Entity;

namespace Repository.v1.IRepository
{
    public interface IRentRepository
    {

        Task<RentEntity> GetById(int id);
    }
}