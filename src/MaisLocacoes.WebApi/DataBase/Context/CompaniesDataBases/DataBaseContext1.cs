using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.DataBase.Context.BaseContext;
using MaisLocacoes.WebApi.Utils.Enums;
using MaisLocacoes.WebApi.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.DataBase.Context.CompaniesDataBases
{
    public class DataBaseContext1 : DataBaseCompanyBaseContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration Configuration;

        public DataBaseContext1() { }

        public DataBaseContext1(string connectionString)
        : base(GetOptions(connectionString))
        { }

        private static DbContextOptions<DbContext> GetOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }

        public DataBaseContext1(DbContextOptions<DbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
                : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
        }

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
            //Definindo valores Unique com Fluent API:           
            modelBuilder.Entity<ContractEntity>()
                .HasIndex(e => e.GuidId)
                .IsUnique();

            //Definindo valor Default IsEditable de ProductTuitionEntity:
            modelBuilder.Entity<ProductTuitionEntity>()
                .Property(p => p.IsEditable)
                .HasDefaultValue(true);

            //Definindo valor Default IsDefault de ProductTuitionValueEntity:
            modelBuilder.Entity<ProductTuitionValueEntity>()
                .Property(p => p.IsDefault)
                .HasDefaultValue(false);

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
            var currenteTimestamp = "CURRENT_TIMESTAMP";

            modelBuilder.Entity<AddressEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<BillEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ClientEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<CompanyTuitionEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<CompanyWasteEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<OsEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ProductEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ProductTuitionEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ProductTuitionValueEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ProductTypeEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<ProductWasteEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<QgEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<RentedPlaceEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<RentEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            modelBuilder.Entity<SupplierEntity>()
                .Property(x => x.CreatedAt)
                .HasDefaultValueSql(currenteTimestamp);

            //Definindo valores Default para o DueDate da Bill:
            modelBuilder.Entity<BillEntity>()
                .Property(x => x.DueDate)
                .HasDefaultValueSql(currenteTimestamp);

            //Definindo valores Default para os deleteds:
            modelBuilder.Entity<BillEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ClientEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<CompanyTuitionEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ContractEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<OsEntity>()
               .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ProductEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ProductTuitionEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<ProductTypeEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false); ;

            modelBuilder.Entity<ProductWasteEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<QgEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<RentEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<SupplierEntity>()
                .Property(x => x.Deleted)
                .HasDefaultValue(false);

            //Definindo valor Default para campo Country em AddressEntity
            modelBuilder.Entity<AddressEntity>()
                .Property(x => x.Country)
                .HasDefaultValue("Brasil");

            //Definindo valor Default para os status de todas entyties            
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

            modelBuilder.Entity<RentEntity>()
              .Property(x => x.Status)
              .HasDefaultValue(RentStatus.RentStatusEnum.ElementAt(0));

            modelBuilder.Entity<ProductTuitionEntity>()
              .Property(x => x.Status)
              .HasDefaultValue(ProductTuitionStatus.ProductTuitionStatusEnum.ElementAt(1));

            //Definindo ForeignKey para as entidades:            
            modelBuilder.Entity<BillEntity>()
            .HasOne(many => many.RentEntity)
            .WithMany(one => one.Bills)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Bills, TableNameEnum.Rents));

            modelBuilder.Entity<BillEntity>()
           .HasOne(many => many.ProductTuitionEntity)
           .WithMany(one => one.Bills)
           .HasForeignKey(many => new { many.ProductTuitionId })
           .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Bills, TableNameEnum.ProductTuitions));

            modelBuilder.Entity<ClientEntity>()
            .HasOne(many => many.AddressEntity)
            .WithMany(one => one.Clients)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Clients, TableNameEnum.Addresses));

            modelBuilder.Entity<OsEntity>()
            .HasOne(many => many.ProductTuitionEntity)
            .WithMany(one => one.Oss)
            .HasForeignKey(many => new { many.ProductTuitionId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Oss, TableNameEnum.ProductTuitions));

            modelBuilder.Entity<ProductEntity>()
            .HasOne(many => many.ProductTypeEntity)
            .WithMany(one => one.Products)
            .HasForeignKey(many => new { many.ProductTypeId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Products, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.ProductTypeEntity)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.ProductTypeId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.ProductEntity)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.ProductId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.Products));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.RentEntity)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.Rents));

            modelBuilder.Entity<ProductTuitionValueEntity>()
            .HasOne(many => many.ProductTypeEntity)
            .WithMany(one => one.ProductTuitionValues)
            .HasForeignKey(many => new { many.ProductTypeId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitionValues, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductWasteEntity>()
            .HasOne(many => many.ProductEntity)
            .WithMany(one => one.ProductWastes)
            .HasForeignKey(many => new { many.ProductId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductWastes, TableNameEnum.Products));

            modelBuilder.Entity<QgEntity>()
            .HasOne(many => many.AddressEntity)
            .WithMany(one => one.Qgs)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Qgs, TableNameEnum.Addresses));

            modelBuilder.Entity<RentedPlaceEntity>()
            .HasOne(many => many.ProductEntity)
            .WithMany(one => one.RentedPlaces)
            .HasForeignKey(many => new { many.ProductId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.RentedPlaces, TableNameEnum.Products));

            modelBuilder.Entity<RentEntity>()
            .HasOne(many => many.AddressEntity)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Addresses));

            modelBuilder.Entity<RentEntity>()
            .HasOne(many => many.ClientEntity)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.ClientId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Clients));

            modelBuilder.Entity<SupplierEntity>()
            .HasOne(many => many.AddressEntity)
            .WithMany(one => one.Suppliers)
            .HasForeignKey(many => new { many.AddressId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Suppliers, TableNameEnum.Addresses));

            modelBuilder.Entity<ContractEntity>()
            .HasOne(many => many.RentEntity)
            .WithMany(one => one.Contracts)
            .HasForeignKey(many => new { many.RentId })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Contracts, TableNameEnum.Rents));

            /*modelBuilder.Entity<CLASSEMUITOS>()
            .HasOne<CLASSEUM>(MUITOS => MUITOS.UM)
            .WithMany(UM => UM.MUITOS)
            .HasForeignKey(MUITOS => new { MUITOS.PROPR1, MUITOS.PROPR2 })
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.MUITOS, TableNameEnum.UM));*/
        }
    }
}
