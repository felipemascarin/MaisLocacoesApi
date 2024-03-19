using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContext3Migrations
{
    public partial class aoassaad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTuitions_Products",
                table: "ProductTuitions");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills",
                column: "ProductTuitionId",
                principalTable: "ProductTuitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTuitions_Products",
                table: "ProductTuitions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTuitions_Products",
                table: "ProductTuitions");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_ProductTuitions",
                table: "Bills",
                column: "ProductTuitionId",
                principalTable: "ProductTuitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTuitions_Products",
                table: "ProductTuitions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
