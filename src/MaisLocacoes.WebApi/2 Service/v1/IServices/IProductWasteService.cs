namespace Service.v1.IServices
{
    public interface IProductWasteService
    {
        Task<bool> DeleteById(int id);
    }
}