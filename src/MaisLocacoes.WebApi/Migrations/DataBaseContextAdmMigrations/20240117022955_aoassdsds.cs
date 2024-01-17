using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContextAdmMigrations
{
    public partial class aoassdsds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Cnpj",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CompanyCnpj",
                table: "Users",
                type: "char(14)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompaniesUsers",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "char(14)", maxLength: 14, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompaniesUsers", x => new { x.Email, x.Cnpj });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyCnpj",
                table: "Users",
                column: "CompanyCnpj");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyCnpj",
                table: "Users",
                column: "CompanyCnpj",
                principalTable: "Companies",
                principalColumn: "Cnpj");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyCnpj",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CompaniesUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyCnpj",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyCnpj",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Users",
                type: "char(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cnpj",
                table: "Users",
                column: "Cnpj");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies",
                table: "Users",
                column: "Cnpj",
                principalTable: "Companies",
                principalColumn: "Cnpj");
        }
    }
}
