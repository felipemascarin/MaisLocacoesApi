using MaisLocacoes.WebApi.DataBase.Context.Adm;
using MaisLocacoes.WebApi.DataBase.Context.BaseContext;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.DataBase.Context.ContextFactory
{
    public class PostgreSqlContextFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _dataBase;

        public PostgreSqlContextFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _dataBase = JwtManager.GetDataBaseByToken(_httpContextAccessor);
        }

        //public DataBaseContextAdm CreateContext()
        //{
        //    var database = JwtManager.ExtractPropertyByToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1], "cnpj");

        //    var connectionString = string.Concat(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=", database, ";");

        //    var optionsBuilder = new DbContextOptionsBuilder<DataBaseContextAdm>();

        //    optionsBuilder.UseNpgsql(connectionString);

        //    return new DataBaseContextAdm(connectionString);
        //}

        public DataBaseContextAdm CreateAdmContext()
        {
            var connectionString = string.Concat(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=maislocacoes;");

            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContextAdm>();

            optionsBuilder.UseNpgsql(connectionString);

            return new DataBaseContextAdm(connectionString);
        }

        public DataBaseCompanyBaseContext CreateContext()
        {
            var connectionString = string.Concat(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=", _dataBase, ";");

            // Use Reflection para encontrar a classe de contexto apropriada
            var contextTypeName = $"DataBaseContext{_dataBase}";
            var contextType = Type.GetType(contextTypeName);

            if (contextType == null)
                throw new InvalidOperationException($"A classe referente ao banco de dados '{contextTypeName}' não foi encontrada no código.");

            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseNpgsql(connectionString);

            // Crie uma instância da classe de contexto encontrada
            var contextInstance = Activator.CreateInstance(contextType, optionsBuilder.Options);

            if (!(contextInstance is DataBaseCompanyBaseContext admContext))
                throw new InvalidOperationException($"A classe de contexto '{contextTypeName}' não herda de DbContext.");

            return admContext;
        }
    }
}
