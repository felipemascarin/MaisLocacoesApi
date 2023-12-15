using MaisLocacoes.WebApi.DataBase.Context.Adm;
using MaisLocacoes.WebApi.DataBase.Context.CompaniesDataBases;
using MaisLocacoes.WebApi.DataBase.Context.ContextFactory;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.Configuration
{
    public class ContextInjetor
    {
        public static void ConfigureContexts(IServiceCollection services, string connectionString)
        {
            services.AddScoped<PostgreSqlContextFactory>();
            services.AddDbContext<DataBaseContextAdm>(options => options.UseNpgsql(string.Concat(connectionString, "Database=maislocacoes;")), ServiceLifetime.Scoped);
            services.AddDbContext<DataBaseContext1>(options => options.UseNpgsql(string.Concat(connectionString, "Database=1;")), ServiceLifetime.Scoped);
            services.AddDbContext<DataBaseContext2>(options => options.UseNpgsql(string.Concat(connectionString, "Database=2;")), ServiceLifetime.Scoped);
            services.AddDbContext<DataBaseContext3>(options => options.UseNpgsql(string.Concat(connectionString, "Database=3;")), ServiceLifetime.Scoped);
        }
    }
}
