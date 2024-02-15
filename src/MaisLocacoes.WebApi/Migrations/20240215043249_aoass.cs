using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaisLocacoes.WebApi.Migrations
{
    public partial class aoass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentedPlaces_Products",
                table: "RentedPlaces");

            migrationBuilder.DropIndex(
                name: "IX_RentedPlaces_ProductId",
                table: "RentedPlaces");

            migrationBuilder.DropColumn(
                name: "ProductParts",
                table: "RentedPlaces");

            migrationBuilder.DropColumn(
                name: "CurrentRentedPlaceId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RentId",
                table: "RentedPlaces",
                newName: "ProductTuitionId");

            migrationBuilder.RenameColumn(
                name: "QgId",
                table: "RentedPlaces",
                newName: "ProductEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "RentedPlaces",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RentedPlaces_ProductEntityId",
                table: "RentedPlaces",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentedPlaces_Products_ProductEntityId",
                table: "RentedPlaces",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentedPlaces_Products_ProductEntityId",
                table: "RentedPlaces");

            migrationBuilder.DropIndex(
                name: "IX_RentedPlaces_ProductEntityId",
                table: "RentedPlaces");

            migrationBuilder.RenameColumn(
                name: "ProductTuitionId",
                table: "RentedPlaces",
                newName: "RentId");

            migrationBuilder.RenameColumn(
                name: "ProductEntityId",
                table: "RentedPlaces",
                newName: "QgId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "RentedPlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductParts",
                table: "RentedPlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CurrentRentedPlaceId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentedPlaces_ProductId",
                table: "RentedPlaces",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentedPlaces_Products",
                table: "RentedPlaces",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
