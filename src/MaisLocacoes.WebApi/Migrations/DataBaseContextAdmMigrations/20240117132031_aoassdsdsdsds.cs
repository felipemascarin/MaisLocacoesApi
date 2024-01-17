using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContextAdmMigrations
{
    public partial class aoassdsdsdsds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Cpf",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers");
        }
    }
}
