using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderIDtoCourierOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CourierOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CourierOrders");
        }
    }
}
