using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delwings.Migrations
{
    /// <inheritdoc />
    public partial class AddedTrackIdToOrdres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Orders");
        }
    }
}
