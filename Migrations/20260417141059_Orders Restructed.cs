using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class OrdersRestructed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysToDeliver",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Courier",
                table: "Orders",
                newName: "CourierName");

            migrationBuilder.AlterColumn<int>(
                name: "InsurancePrice",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OperatorPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorPlaces", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperatorPlaces");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CourierName",
                table: "Orders",
                newName: "Courier");

            migrationBuilder.AlterColumn<double>(
                name: "InsurancePrice",
                table: "Orders",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysToDeliver",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "Orders",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }
    }
}
