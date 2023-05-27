namespace Service.v1.IServices
{
    public interface IQgService
    {
        Task<bool> DeleteById(int id);
    }
}