using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class OrderLocationTypeChangedToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Places_CurrentLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrentLocationId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
