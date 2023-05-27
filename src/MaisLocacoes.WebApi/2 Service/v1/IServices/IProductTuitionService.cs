namespace Service.v1.IServices
{
    public interface IProductTuitionService
    {
        Task<bool> DeleteById(int id);
    }
}