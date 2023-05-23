using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.IRepository;
using System.Net;

namespace Repository.v1.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly PostgreSqlContext _context;

        public BillRepository(PostgreSqlContext context)
        {
            _context = context;
        }
    }
}