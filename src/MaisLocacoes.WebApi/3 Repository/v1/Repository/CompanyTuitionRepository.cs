using MaisLocacoes.WebApi.Context;
using Repository.v1.IRepository;

namespace Repository.v1.Repository
{
    public class CompanyTuitionRepository : ICompanyTuitionRepository
    {
        private readonly PostgreSqlContext _context;

        public CompanyTuitionRepository(PostgreSqlContext context)
        {
            _context = context;
        }
    }
}