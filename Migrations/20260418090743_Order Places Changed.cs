using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class OrderPlacesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLocationId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrentLocationId",
                table: "Orders",
                column: "CurrentLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Places_CurrentLocationId",
                table: "Orders",
                column: "CurrentLocationId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Places_CurrentLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrentLocationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrentLocationId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
