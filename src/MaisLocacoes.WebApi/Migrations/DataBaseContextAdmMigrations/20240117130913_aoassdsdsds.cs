using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContextAdmMigrations
{
    public partial class aoassdsdsds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompaniesUsers_Cnpj",
                table: "CompaniesUsers",
                column: "Cnpj");

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesUsers_Companies",
                table: "CompaniesUsers",
                column: "Cnpj",
                principalTable: "Companies",
                principalColumn: "Cnpj",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesUsers_Companies",
                table: "CompaniesUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompaniesUsers_Cnpj",
                table: "CompaniesUsers");
        }
    }
}
