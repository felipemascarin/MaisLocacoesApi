using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations.DataBaseContext1Migrations
{
    public partial class aoassz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentedPlaceId",
                table: "Qgs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Qgs_RentedPlaceId",
                table: "Qgs",
                column: "RentedPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Qgs_RentedPlaces",
                table: "Qgs",
                column: "RentedPlaceId",
                principalTable: "RentedPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qgs_RentedPlaces",
                table: "Qgs");

            migrationBuilder.DropIndex(
                name: "IX_Qgs_RentedPlaceId",
                table: "Qgs");

            migrationBuilder.DropColumn(
                name: "RentedPlaceId",
                table: "Qgs");
        }
    }
}
