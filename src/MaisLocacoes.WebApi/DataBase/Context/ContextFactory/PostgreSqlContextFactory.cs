using MaisLocacoes.WebApi.DataBase.Context.DataBases;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.DataBase.Context.ContextFactory
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

        public DataBaseContextAdm CreateAdmContext()
        {
            return new DataBaseContextAdm(_configuration);
        }

        public DbContext CreateContext()
        {
            // Use Reflection para encontrar a classe de contexto apropriada
            var contextTypeName = _configuration["MyPostgreSqlConnection:DirectoryCompaniesDataBasesContexts"] + JwtManager.GetDataBaseByToken(_httpContextAccessor);
            var contextType = Type.GetType(contextTypeName);
            
            if (contextType == null)
                throw new InvalidOperationException($"A classe referente ao banco de dados '{contextTypeName}' não foi encontrada no código.");

            // Crie uma instância da classe de contexto encontrada
            var contextInstance = Activator.CreateInstance(contextType, _configuration);

            if (!(contextInstance is DbContext admContext))
                throw new InvalidOperationException($"A classe de contexto '{contextTypeName}' não herda de DbContext.");

            return admContext;
        }
    }
}
