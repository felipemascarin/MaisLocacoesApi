namespace Service.v1.IServices
{
    public interface ISupplierService
    {
        Task<bool> DeleteById(int id);
    }
}