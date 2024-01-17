namespace MaisLocacoes.WebApi._2Service.v1.IServices.UserSchema
{
    public interface ICompanyUserService
    {
        Task<IEnumerable<string>> GetCnpjListByEmail(string email);
    }
}
