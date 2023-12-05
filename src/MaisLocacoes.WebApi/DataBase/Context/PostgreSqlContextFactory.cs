using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.DataBase.Context
{
    public class PostgreSqlContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public PostgreSqlContextFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public PostgreSqlContext CreateContext()
        {
            var database = JwtManager.ExtractPropertyByToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1], "cnpj");

            var connectionString = string.Concat(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=", database, ";");

            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreSqlContext(connectionString, _httpContextAccessor, _configuration);
        }

        public PostgreSqlContext CreateContext(string database)
        {
            var connectionString = string.Concat(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=", database, ";");

            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreSqlContext(connectionString, _httpContextAccessor, _configuration);
        }
    }
}
