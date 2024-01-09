using MaisLocacoes.WebApi.DataBase.Context.DataBasesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.DataBase.Context.CompaniesDataBases
{
    public class DataBaseContext1 : DataBasesContextBase
    {
        private readonly string _database = "Database=1";

        private readonly IConfiguration _configuration;

        public DataBaseContext1(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"] + _database);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DataBasesConfigurations.CompaniesDataBasesConfigurations.DataBaseConfigurations(modelBuilder);
        }
    }
}
