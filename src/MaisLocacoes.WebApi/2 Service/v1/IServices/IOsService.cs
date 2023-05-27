namespace Service.v1.IServices
{
    public interface IOsService
    {
        Task<bool> DeleteById(int id);
    }
}