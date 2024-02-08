using MaisLocacoes.WebApi._2Service.v1.IServices.UserSchema;
using MaisLocacoes.WebApi._3Repository.v1.IRepository.UserSchema;
using System.Net;

namespace MaisLocacoes.WebApi._2Service.v1.Services.UserSchema
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly ICompanyUserRepository _companyUserRepository;

        public CompanyUserService(ICompanyUserRepository companyUserRepository)
        {
            _companyUserRepository = companyUserRepository;
        }
    }
}
