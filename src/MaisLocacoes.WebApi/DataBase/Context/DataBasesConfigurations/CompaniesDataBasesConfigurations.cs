using MaisLocacoes.WebApi._3Repository.v1.Entity;
using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity;

namespace MaisLocacoes.WebApi.DataBase.Context.DataBasesConfigurations
{
    public static class CompaniesDataBasesConfigurations
    {
        public static void DataBaseConfigurations(ModelBuilder modelBuilder)
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

            //modelBuilder.Entity<ProductEntity>()
            //    .Property(p => p.BoughtValue)
            //    .HasDefaultValue(0);

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
            .HasOne(many => many.Rent)
            .WithMany(one => one.Bills)
            .HasForeignKey(many => new { many.RentId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Bills, TableNameEnum.Rents));

            modelBuilder.Entity<BillEntity>()
            .HasOne(many => many.ProductTuition)
            .WithMany(one => one.Bills)
            .HasForeignKey(many => new { many.ProductTuitionId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Bills, TableNameEnum.ProductTuitions));

            modelBuilder.Entity<ClientEntity>()
            .HasOne(many => many.Address)
            .WithMany(one => one.Clients)
            .HasForeignKey(many => new { many.AddressId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Clients, TableNameEnum.Addresses));

            modelBuilder.Entity<OsEntity>()
            .HasOne(many => many.ProductTuition)
            .WithMany(one => one.Oss)
            .HasForeignKey(many => new { many.ProductTuitionId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Oss, TableNameEnum.ProductTuitions));

            modelBuilder.Entity<ProductEntity>()
            .HasOne(many => many.ProductType)
            .WithMany(one => one.Products)
            .HasForeignKey(many => new { many.ProductTypeId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Products, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.ProductType)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.ProductTypeId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.Product)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.ProductId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.Products));

            modelBuilder.Entity<ProductTuitionEntity>()
            .HasOne(many => many.Rent)
            .WithMany(one => one.ProductTuitions)
            .HasForeignKey(many => new { many.RentId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitions, TableNameEnum.Rents));

            modelBuilder.Entity<ProductTuitionValueEntity>()
            .HasOne(many => many.ProductType)
            .WithMany(one => one.ProductTuitionValues)
            .HasForeignKey(many => new { many.ProductTypeId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductTuitionValues, TableNameEnum.ProductTypes));

            modelBuilder.Entity<ProductWasteEntity>()
            .HasOne(many => many.Product)
            .WithMany(one => one.ProductWastes)
            .HasForeignKey(many => new { many.ProductId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.ProductWastes, TableNameEnum.Products));

            modelBuilder.Entity<QgEntity>()
            .HasOne(many => many.Address)
            .WithMany(one => one.Qgs)
            .HasForeignKey(many => new { many.AddressId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Qgs, TableNameEnum.Addresses));

            modelBuilder.Entity<RentEntity>()
            .HasOne(many => many.Address)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.AddressId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Addresses));

            modelBuilder.Entity<RentEntity>()
            .HasOne(many => many.Client)
            .WithMany(one => one.Rents)
            .HasForeignKey(many => new { many.ClientId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Rents, TableNameEnum.Clients));

            modelBuilder.Entity<SupplierEntity>()
            .HasOne(many => many.Address)
            .WithMany(one => one.Suppliers)
            .HasForeignKey(many => new { many.AddressId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Suppliers, TableNameEnum.Addresses));

            modelBuilder.Entity<ContractEntity>()
            .HasOne(many => many.Rent)
            .WithMany(one => one.Contracts)
            .HasForeignKey(many => new { many.RentId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Contracts, TableNameEnum.Rents));

            modelBuilder.Entity<QgEntity>()
            .HasOne(many => many.RentedPlace)
            .WithMany(one => one.Qgs)
            .HasForeignKey(many => new { many.RentedPlaceId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Qgs, TableNameEnum.RentedPlaces));

            modelBuilder.Entity<RentedPlaceEntity>()
            .HasOne(many => many.ProductTuition)
            .WithMany(one => one.RentedPlaces)
            .HasForeignKey(many => new { many.ProductTuitionId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.RentedPlaces, TableNameEnum.ProductTuitions));

            modelBuilder.Entity<RentedPlaceEntity>()
            .HasOne(many => many.Product)
            .WithMany(one => one.RentedPlaces)
            .HasForeignKey(many => new { many.ProductId })
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.RentedPlaces, TableNameEnum.Products));

            /*modelBuilder.Entity<CLASSEMUITOS>()
            .HasOne<CLASSEUM>(MUITOS => MUITOS.UM)
            .WithMany(UM => UM.MUITOS)
            .HasForeignKey(MUITOS => new { MUITOS.PROPR1, MUITOS.PROPR2 })
            .OnDelete(DeleteBehavior.escolher) - se preciso
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.MUITOS, TableNameEnum.UM));*/
        }
    }
}
