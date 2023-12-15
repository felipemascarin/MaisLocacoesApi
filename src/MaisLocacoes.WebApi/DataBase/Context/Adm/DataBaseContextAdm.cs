using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi.DataBase.Context.Adm
{
    public class DataBaseContextAdm : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration Configuration;

        public DataBaseContextAdm() { }

        public DataBaseContextAdm(string connectionString)
        : base(GetOptions(connectionString))
        { }

        private static DbContextOptions<DataBaseContextAdm> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContextAdm>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }

        public DataBaseContextAdm(DbContextOptions<DataBaseContextAdm> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
                : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        public DbSet<CompanyEntity> Companies { get; set; }

        public DbSet<CompanyAddressEntity> CompaniesAddresses { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var database = JwtManager.ExtractPropertyByToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1], "database");

                    var connectionString = string.Concat(Configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "Database=", database, ";");

                    optionsBuilder.UseNpgsql(connectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Definindo Schema de User - define o schema que a tabela existe:
            modelBuilder.Entity<CompanyEntity>(entity =>
            {
                entity.ToTable(nameof(TableNameEnum.Companies), "users");
            });

            modelBuilder.Entity<CompanyAddressEntity>(entity =>
            {
                entity.ToTable(nameof(TableNameEnum.CompanyAddress), "users");
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable(nameof(TableNameEnum.Users), "users");
            });

            //Definindo valores Unique com Fluent API:
            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(e => e.Email)
                .IsUnique();

            //Definindo valor Default NotifyDaysBefore de CompanyEntity:
            modelBuilder.Entity<CompanyEntity>()
                .Property(p => p.NotifyDaysBefore)
                .HasDefaultValue(0);

            //Definindo valores Default para campos CreatedAt como horario de inserção em UTC:
            var currenteTimestamp = "CURRENT_TIMESTAMP";

            modelBuilder.Entity<CompanyAddressEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<CompanyEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            //Definindo valor Default para os status de todas entyties
            modelBuilder.Entity<CompanyEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(CompanyStatus.CompanyStatusEnum.ElementAt(0));

            modelBuilder.Entity<UserEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(UserStatus.UserStatusEnum.ElementAt(0));

            //Definindo ForeignKey para as entidades:
            modelBuilder.Entity<CompanyEntity>()
            .HasOne(many => many.CompanyAddressEntity)
            .WithMany(one => one.Companies)
            .HasForeignKey(many => new { many.CompanyAddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Companies, TableNameEnum.CompanyAddress));

            modelBuilder.Entity<UserEntity>()
            .HasOne(many => many.CompanyEntity)
            .WithMany(one => one.Users)
            .HasForeignKey(many => new { many.Cnpj })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Users, TableNameEnum.Companies));

            /*modelBuilder.Entity<CLASSEMUITOS>()
            .HasOne<CLASSEUM>(MUITOS => MUITOS.UM)
            .WithMany(UM => UM.MUITOS)
            .HasForeignKey(MUITOS => new { MUITOS.PROPR1, MUITOS.PROPR2 })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.MUITOS, TableNameEnum.UM));*/
        }
    }
}