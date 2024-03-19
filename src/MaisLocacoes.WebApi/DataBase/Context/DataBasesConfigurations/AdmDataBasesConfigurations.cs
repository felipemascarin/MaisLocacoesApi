using MaisLocacoes.WebApi._3Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Context;
using MaisLocacoes.WebApi.Repository.v1.Entity.UserSchema;
using MaisLocacoes.WebApi.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.v1.Entity.UserSchema;

namespace MaisLocacoes.WebApi.DataBase.Context.CompaniesDataBasesConfigurations
{
    public static class AdmDataBasesConfigurations

    {
        public static void AdmDataBaseConfigurations(ModelBuilder modelBuilder)
        {
            //Definindo valores Unique com Fluent API:
            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(c => c.DataBase)
                .IsUnique();

            modelBuilder.Entity<CompanyEntity>()
            .HasIndex(c => c.CompanyName)
            .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(c => c.Cpf)
                .IsUnique();

            //Definindo valor Default NotifyDaysBefore de CompanyEntity:
            modelBuilder.Entity<CompanyEntity>()
                .Property(p => p.NotifyDaysBefore)
                .HasDefaultValue(0);

            //Definindochave primária composta para CompaniesUsers:
            modelBuilder.Entity<CompanyUserEntity>()
                .HasKey(c => new { c.Email, c.Cnpj });

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
            .HasOne(many => many.CompanyAddress)
            .WithMany(one => one.Companies)
            .HasForeignKey(many => new { many.CompanyAddressId })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.Companies, TableNameEnum.CompaniesAddresses));

            modelBuilder.Entity<CompanyUserEntity>()
            .HasOne(many => many.User)
            .WithMany(one => one.CompaniesUsers)
            .HasForeignKey(many => new { many.Email })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.CompaniesUsers, TableNameEnum.Users));

            modelBuilder.Entity<CompanyUserEntity>()
            .HasOne(many => many.Company)
            .WithMany(one => one.CompaniesUsers)
            .HasForeignKey(many => new { many.Cnpj })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.CompaniesUsers, TableNameEnum.Companies));

            /*modelBuilder.Entity<CLASSEMUITOS>()
            .HasOne<CLASSEUM>(MUITOS => MUITOS.UM)
            .WithMany(UM => UM.MUITOS)
            .HasForeignKey(MUITOS => new { MUITOS.PROPR1, MUITOS.PROPR2 })
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName(ForeignKeyNameCreator.CreateForeignKeyName(TableNameEnum.MUITOS, TableNameEnum.UM));*/
        }
    }
}
