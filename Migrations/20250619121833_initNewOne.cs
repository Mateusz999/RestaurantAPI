using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurationAPI.Migrations
{
    /// <inheritdoc />
    public partial class initNewOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Restaurants_RestaurantId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_RestaurantId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_AdressId",
                table: "Restaurants",
                column: "AdressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Addresses_AdressId",
                table: "Restaurants",
                column: "AdressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Addresses_AdressId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_AdressId",
                table: "Restaurants");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RestaurantId",
                table: "Addresses",
                column: "RestaurantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Restaurants_RestaurantId",
                table: "Addresses",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
