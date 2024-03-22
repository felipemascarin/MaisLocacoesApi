using Microsoft.EntityFrameworkCore;

namespace MaisLocacoes.WebApi.DataBase.Context.DataBases
{
    public class DataBaseContext2 : DataBasesContextBase
    {
        private readonly string _database = "Database=2";

        private readonly IConfiguration _configuration;

        public DataBaseContext2(IConfiguration configuration)
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
