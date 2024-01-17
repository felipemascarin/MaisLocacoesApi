using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContextAdmMigrations
{
    public partial class aoassdsdsdsdsad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cpf",
                table: "Users",
                column: "Cpf",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Cpf",
                table: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Cpf");

            migrationBuilder.AddForeignKey(
                name: "FK_CompaniesUsers_Users",
                table: "CompaniesUsers",
                column: "Email",
                principalTable: "Users",
                principalColumn: "Cpf",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
