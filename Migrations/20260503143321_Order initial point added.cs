using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class Orderinitialpointadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitialCity",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialCity",
                table: "Orders");
        }
    }
}
