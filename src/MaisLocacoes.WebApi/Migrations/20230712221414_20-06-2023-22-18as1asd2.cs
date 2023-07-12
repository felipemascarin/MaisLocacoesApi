using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations
{
    public partial class _200620232218as1asd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bills_ProductTuitionId",
                table: "Bills",
                column: "ProductTuitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills",
                column: "ProductTuitionId",
                principalTable: "ProductTuitions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_ProductTuitionId",
                table: "Bills");
        }
    }
}
