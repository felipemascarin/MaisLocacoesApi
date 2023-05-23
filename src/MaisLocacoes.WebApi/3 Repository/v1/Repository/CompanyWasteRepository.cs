using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Helpers;
using Repository.v1.IRepository;
using System.Net;

namespace Repository.v1.Repository
{
    public class CompanyWasteRepository : ICompanyWasteRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyWasteRepository(PostgreSqlContext context)
        {
            _context = context;
        }
    }
}