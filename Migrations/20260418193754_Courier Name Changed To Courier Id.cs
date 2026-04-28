using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class CourierNameChangedToCourierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourierName",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CourierId",
                table: "Orders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourierId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "CourierName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
