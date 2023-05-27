namespace Service.v1.IServices
{
    public interface ICompanyTuitionService
    {
        Task<bool> DeleteById(int id);
    }
}