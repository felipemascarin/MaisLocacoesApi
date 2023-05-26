using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations
{
    public partial class _2023052123423 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "users");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cep = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Street = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Complement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    District = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Country = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValue: "Brasil"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillsDeletions",
                columns: table => new
                {
                    BillsDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentId = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NfIdFireBase = table.Column<int>(type: "integer", nullable: true),
                    PaymentMode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillsDeletions", x => x.BillsDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "ClientsDeletions",
                columns: table => new
                {
                    ClientsDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    Cpf = table.Column<string>(type: "char(11)", maxLength: 11, nullable: true),
                    Rg = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ClientName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    StateRegister = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FantasyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BornDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Career = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CivilStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Segment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CpfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CnpjDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    AddressDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ClientPictureUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsDeletions", x => x.ClientsDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAddress",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cep = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Street = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Number = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Complement = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    District = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Country = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTuitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AsaasNumber = table.Column<int>(type: "integer", nullable: true),
                    TuitionNumber = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTuitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTuitionsDeletions",
                columns: table => new
                {
                    CompanyTuitionsDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AsaasNumber = table.Column<int>(type: "integer", nullable: true),
                    TuitionNumber = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTuitionsDeletions", x => x.CompanyTuitionsDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyWastes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyWastes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyWastesDeletions",
                columns: table => new
                {
                    CompanyWastesDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyWastesDeletions", x => x.CompanyWastesDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "ProductsDeletions",
                columns: table => new
                {
                    ProductsDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProductType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    SupplierId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DateBought = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BoughtValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CurrentRentedPlaceId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDeletions", x => x.ProductsDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTuitionsDeletions",
                columns: table => new
                {
                    ProductTuitionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentId = table.Column<int>(type: "integer", nullable: true),
                    ProductType = table.Column<string>(type: "text", nullable: true),
                    ProductCode = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    InitialDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinalDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Parts = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTuitionsDeletions", x => x.ProductTuitionsId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedByanderson = table.Column<string>(type: "text", nullable: true),
                    IsManyParts = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypesDeletions",
                columns: table => new
                {
                    ProductTypesDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypesDeletions", x => x.ProductTypesDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "ProductWastes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProductType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWastes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductWastesDeletions",
                columns: table => new
                {
                    ProductWastesDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProductType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWastesDeletions", x => x.ProductWastesDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "QgsDeletions",
                columns: table => new
                {
                    QgsDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QgsDeletions", x => x.QgsDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "RentedPlacesDeletions",
                columns: table => new
                {
                    RentedPlacesDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductType = table.Column<string>(type: "text", nullable: true),
                    ProductCode = table.Column<string>(type: "text", nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    RentId = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentedPlacesDeletions", x => x.RentedPlacesDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "SuppliersDeletions",
                columns: table => new
                {
                    SuppliersDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuppliersDeletions", x => x.SuppliersDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "UsersDeletions",
                schema: "users",
                columns: table => new
                {
                    PeopleDeletionsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cpf = table.Column<string>(type: "char(11)", maxLength: 11, nullable: true),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    Rg = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    BornDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CivilStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CpfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDeletions", x => x.PeopleDeletionsId);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Rg = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ClientName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    StateRegister = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FantasyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BornDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Career = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CivilStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Segment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CpfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CnpjDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    AddressDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ClientPictureUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValue: "regular"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Qgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qgs_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "users",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: false),
                    CompanyAddressId = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StateRegister = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FantasyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Tel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Segment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PjDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    AddressDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    NotifyDaysBefore = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValue: "regular"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_Companies_CompaniesAddresses",
                        column: x => x.CompanyAddressId,
                        principalSchema: "users",
                        principalTable: "CompanyAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProductType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DateBought = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BoughtValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "free"),
                    CurrentRentedPlaceId = table.Column<int>(type: "integer", nullable: true),
                    Parts = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    RentedParts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.ProductType, x.Code });
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes",
                        column: x => x.ProductType,
                        principalTable: "ProductTypes",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Carriage = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rents_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_Clients",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "users",
                columns: table => new
                {
                    Cpf = table.Column<string>(type: "char(11)", maxLength: 11, nullable: false),
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: true),
                    Rg = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    BornDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CivilStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CpfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValue: "regular"),
                    LastToken = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Cpf);
                    table.ForeignKey(
                        name: "FK_Users_Companies",
                        column: x => x.Cnpj,
                        principalSchema: "users",
                        principalTable: "Companies",
                        principalColumn: "Cnpj");
                });

            migrationBuilder.CreateTable(
                name: "RentedPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductType = table.Column<string>(type: "character varying(255)", nullable: true),
                    ProductCode = table.Column<string>(type: "character varying(255)", nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    RentId = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentedPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentedPlaces_Addresses",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentedPlaces_Products",
                        columns: x => new { x.ProductType, x.ProductCode },
                        principalTable: "Products",
                        principalColumns: new[] { "ProductType", "Code" });
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "open"),
                    NfIdFireBase = table.Column<int>(type: "integer", nullable: true),
                    PaymentMode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bills_Rents",
                        column: x => x.RentId,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Oss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryCpf = table.Column<string>(type: "char(11)", maxLength: 11, nullable: true),
                    RentId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "waiting"),
                    InitialDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinalDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeliveryObservation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oss_Rents",
                        column: x => x.RentId,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTuitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RentId = table.Column<int>(type: "integer", nullable: false),
                    ProductType = table.Column<string>(type: "character varying(255)", nullable: true),
                    ProductCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    InitialDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinalDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Parts = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTuitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTuitions_ProductTypes",
                        column: x => x.ProductType,
                        principalTable: "ProductTypes",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_ProductTuitions_Rents",
                        column: x => x.RentId,
                        principalTable: "Rents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_RentId",
                table: "Bills",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AddressId",
                table: "Clients",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyAddressId",
                schema: "users",
                table: "Companies",
                column: "CompanyAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Email",
                schema: "users",
                table: "Companies",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Oss_RentId",
                table: "Oss",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTuitions_ProductType",
                table: "ProductTuitions",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTuitions_RentId",
                table: "ProductTuitions",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_Qgs_AddressId",
                table: "Qgs",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_RentedPlaces_AddressId",
                table: "RentedPlaces",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_RentedPlaces_ProductType_ProductCode",
                table: "RentedPlaces",
                columns: new[] { "ProductType", "ProductCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Rents_AddressId",
                table: "Rents",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_ClientId",
                table: "Rents",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_AddressId",
                table: "Suppliers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cnpj",
                schema: "users",
                table: "Users",
                column: "Cnpj");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "users",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BillsDeletions");

            migrationBuilder.DropTable(
                name: "ClientsDeletions");

            migrationBuilder.DropTable(
                name: "CompanyTuitions");

            migrationBuilder.DropTable(
                name: "CompanyTuitionsDeletions");

            migrationBuilder.DropTable(
                name: "CompanyWastes");

            migrationBuilder.DropTable(
                name: "CompanyWastesDeletions");

            migrationBuilder.DropTable(
                name: "Oss");

            migrationBuilder.DropTable(
                name: "ProductsDeletions");

            migrationBuilder.DropTable(
                name: "ProductTuitions");

            migrationBuilder.DropTable(
                name: "ProductTuitionsDeletions");

            migrationBuilder.DropTable(
                name: "ProductTypesDeletions");

            migrationBuilder.DropTable(
                name: "ProductWastes");

            migrationBuilder.DropTable(
                name: "ProductWastesDeletions");

            migrationBuilder.DropTable(
                name: "Qgs");

            migrationBuilder.DropTable(
                name: "QgsDeletions");

            migrationBuilder.DropTable(
                name: "RentedPlaces");

            migrationBuilder.DropTable(
                name: "RentedPlacesDeletions");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "SuppliersDeletions");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "users");

            migrationBuilder.DropTable(
                name: "UsersDeletions",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "CompanyAddress",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
