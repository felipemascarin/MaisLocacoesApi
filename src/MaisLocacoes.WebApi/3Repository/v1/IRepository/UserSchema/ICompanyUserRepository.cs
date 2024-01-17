using MaisLocacoes.WebApi._3Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi._3Repository.v1.IRepository.UserSchema
{
    public interface ICompanyUserRepository
    {
        Task<CompanyUserEntity> CreateCompanyUser(CompanyUserEntity companyUserEntity);
        Task<IEnumerable<string>> GetCnpjListByEmail(string email);
        Task<IEnumerable<string>> GetEmailListByCnpj(string cnpj);
        Task DeleteCompanyUser(CompanyUserEntity userForDelete);
    }
}
