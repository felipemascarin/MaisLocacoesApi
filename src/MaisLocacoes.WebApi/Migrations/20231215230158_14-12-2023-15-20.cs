using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations
{
    public partial class _141220231520 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "users");

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
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAddress", x => x.Id);
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
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, defaultValue: "regular"),
                    Module = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TimeZone = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    BornDate = table.Column<DateTime>(type: "date", nullable: true),
                    Cel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CivilStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CpfDocumentUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, defaultValue: "regular"),
                    LastToken = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                name: "IX_Users_Cnpj",
                schema: "users",
                table: "Users",
                column: "Cnpj");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "users");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "users");

            migrationBuilder.DropTable(
                name: "CompanyAddress",
                schema: "users");
        }
    }
}
