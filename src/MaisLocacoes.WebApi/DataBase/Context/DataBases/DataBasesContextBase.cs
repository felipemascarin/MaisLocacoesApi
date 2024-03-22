using MaisLocacoes.WebApi._3Repository.v1.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.DataBase.Context.DataBases
{
    public class DataBasesContextBase : DbContext
    {
        public DataBasesContextBase() { }

        public DataBasesContextBase(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        { }

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<BillEntity> Bills { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<CompanyTuitionEntity> CompanyTuitions { get; set; }
        public DbSet<CompanyWasteEntity> CompanyWastes { get; set; }
        public DbSet<ContractEntity> Contracts { get; set; }
        public DbSet<OsEntity> Oss { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductTuitionEntity> ProductTuitions { get; set; }
        public DbSet<ProductTuitionValueEntity> ProductTuitionValues { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes { get; set; }
        public DbSet<ProductWasteEntity> ProductWastes { get; set; }
        public DbSet<QgEntity> Qgs { get; set; }
        public DbSet<RentedPlaceEntity> RentedPlaces { get; set; }
        public DbSet<RentEntity> Rents { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
    }
}
