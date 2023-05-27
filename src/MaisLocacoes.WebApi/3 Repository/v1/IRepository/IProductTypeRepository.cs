namespace Repository.v1.IRepository
{
    public interface IProductTypeRepository
    {
        Task<bool> ProductTypeExists(string type);
    }
}