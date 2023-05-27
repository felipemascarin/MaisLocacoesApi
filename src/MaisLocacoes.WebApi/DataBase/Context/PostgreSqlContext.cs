using MaisLocacoes.WebApi.DataBase;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi.Context
{
    public class PostgreSqlContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration Configuration;
        public PostgreSqlContext() { }

        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
                : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<BillEntity> Bills { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyAddressEntity> CompaniesAddresses { get; set; }
        public DbSet<CompanyTuitionEntity> CompanyTuitions { get; set; }
        public DbSet<CompanyWasteEntity> CompanyWastes { get; set; }
        public DbSet<OsEntity> Oss { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductTuitionEntity> ProductTuitions { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes { get; set; }
        public DbSet<ProductWasteEntity> ProductWastes { get; set; }
        public DbSet<QgEntity> Qgs { get; set; }
        public DbSet<RentedPlaceEntity> RentedPlaces { get; set; }
        public DbSet<RentEntity> Rents { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    var schema = JwtManager.ExtractSchemaByToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1]);

                    var connectionString = string.Concat(Configuration["MyPostgreSqlConnection:MyPostgreSqlConnectionString"], "SearchPath=", schema, ";");

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

            modelBuilder.Entity<UserEntity>()
                .HasIndex(e => e.Email)
                .IsUnique();

            //Definindo chaves compostas:
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => new { p.ProductType, p.Code });


            //Definindo valor Default NotifyDaysBefore de CompanyEntity:
            modelBuilder.Entity<CompanyEntity>()
                .Property(p => p.NotifyDaysBefore)
                .HasDefaultValue(0);

            //Definindo valor Default para Parts no Product:
            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.Parts)
                .HasDefaultValue(1);

            modelBuilder.Entity<ProductEntity>()
                .Property(p => p.RentedParts)
                .HasDefaultValue(0);

            //Definindo valor Default Parts de ProductTuitionEntity:
            modelBuilder.Entity<ProductTuitionEntity>()
                .Property(p => p.Parts)
                .HasDefaultValue(1);

            //Definindo valores Default para campos CreatedAt como horario de inserção em UTC:
            var currenteTimestampUtc = "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'";

            modelBuilder.Entity<CompanyEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<UserEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<AddressEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<BillEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<ClientEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<CompanyTuitionEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<CompanyWasteEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<OsEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<ProductEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<ProductTuitionEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<ProductTypeEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<ProductWasteEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<QgEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<RentedPlaceEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<RentEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            modelBuilder.Entity<SupplierEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestampUtc);

            //Definindo valor Default para campo Country em AddressEntity
            modelBuilder.Entity<AddressEntity>()
                .Property(x => x.Country)
                .HasDefaultValue("Brasil");

            //Definindo valor Default para os status de todas entyties
            modelBuilder.Entity<CompanyEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(CompanyStatus.CompanyStatusEnum.ElementAt(0));

            modelBuilder.Entity<UserEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(UserStatus.UserStatusEnum.ElementAt(0));

            modelBuilder.Entity<BillEntity>()
                .Property(x => x.Status)
                .HasDefaultValue(BillStatus.BillStatusEnum.ElementAt(0));

            modelBuilder.Entity<OsEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(OsStatus.OsStatusEnum.ElementAt(0));

            modelBuilder.Entity<ProductEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(ProductStatus.ProductStatusEnum.ElementAt(0));

            modelBuilder.Entity<ClientEntity>()
               .Property(x => x.Status)
               .HasDefaultValue(ClientStatus.ClientStatusEnum.ElementAt(0));

           
            //Definindo ForeignKey para as entidades:

            modelBuilder.Entity<CompanyEntity>()
            .HasOne<CompanyAddressEntity>(many => many.CompanyAddressEntity)
            .WithMany(one => one.Companies)
            .HasForeignKey(many => new { many.CompanyAddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Companies, TableNameEnum.CompanyAddress));

            modelBuilder.Entity<UserEntity>()
            .HasOne<CompanyEntity>(many => many.CompanyEntity)
            .WithMany(one => one.Users)
            .HasForeignKey(many => new { many.Cnpj })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Users, TableNameEnum.Companies));

            modelBuilder.Entity<BillEntity>()
            .HasOne<RentEntity>(many => many.RentEntity)
            .WithMany(one => one.Bills)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Bills, TableNameEnum.Rents));

            modelBuilder.Entity<ClientEntity>()
            .HasOne<AddressEntity>(many => many.AddressEntity)
            .WithMany(one => one.Clients)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Clients, TableNameEnum.Addresses));

            modelBuilder.Entity<OsEntity>()
            .HasOne<RentEntity>(many => many.RentEntity)
            .WithMany(one => one.Oss)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Oss, TableNameEnum.Rents));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne<ProductTypeEntity>(many => many.ProductTypeEntity)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.ProductType })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne<RentEntity>(many => many.RentEntity)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.Rents));

            modelBuilder.Entity<ProductEntity>()
            .HasOne<ProductTypeEntity>(many => many.ProductTypeEntity)
            .WithMany(one => one.Products)
            .HasForeignKey(many => new { many.ProductType })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Products, TableNameEnum.ProductTypes));

            modelBuilder.Entity<QgEntity>()
            .HasOne<AddressEntity>(many => many.AddressEntity)
            .WithMany(one => one.Qgs)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Qgs, TableNameEnum.Addresses));

            modelBuilder.Entity<RentedPlaceEntity>()
            .HasOne<AddressEntity>(many => many.AddressEntity)
            .WithMany(one => one.RentedPlaces)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.RentedPlaces, TableNameEnum.Addresses));

            modelBuilder.Entity<RentedPlaceEntity>()
            .HasOne<ProductEntity>(many => many.ProductEntity)
            .WithMany(one => one.RentedPlaces)
            .HasForeignKey(many => new { many.ProductType, many.ProductCode })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.RentedPlaces, TableNameEnum.Products));

            modelBuilder.Entity<RentEntity>()
            .HasOne<AddressEntity>(many => many.AddressEntity)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Addresses));

            modelBuilder.Entity<RentEntity>()
            .HasOne<ClientEntity>(many => many.ClientEntity)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.ClientId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Clients));

            modelBuilder.Entity<SupplierEntity>()
            .HasOne<AddressEntity>(many => many.AddressEntity)
            .WithMany(one => one.Suppliers)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Suppliers, TableNameEnum.Addresses));

            /*modelBuilder.Entity<CLASSEMUITOS>()
            .HasOne<CLASSEUM>(MUITOS => MUITOS.UM)
            .WithMany(UM => UM.MUITOS)
            .HasForeignKey(MUITOS => new { MUITOS.PROPR1, MUITOS.PROPR2 })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.MUITOS, TableNameEnum.UM));*/
        }

        public void CreateSchema(string schemaName)
        {
            this.Database.ExecuteSqlRaw(NewSchemaSqlCreator.SqlQueryForNewSchema(schemaName));
            this.Database.ExecuteSqlRaw(NewSchemaSqlCreator.SqlQueryForAtualizeSchemasTablesAndFks());
        }
    }
}