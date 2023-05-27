namespace Service.v1.IServices
{
    public interface ICompanyWasteService
    {
        Task<bool> DeleteById(int id);
    }
}