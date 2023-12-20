using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi.DataBase.Context.Adm
{
    public class DataBaseContextAdm : DbContext
    {
        private readonly string _database = "Database=maislocacoes";

        private readonly IConfiguration _configuration;

        public DataBaseContextAdm(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyAddressEntity> CompaniesAddresses { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"] + _database);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CompaniesDataBasesConfigurations.AdmDataBasesConfigurations.AdmDataBaseConfigurations(modelBuilder);
        }
    }
}