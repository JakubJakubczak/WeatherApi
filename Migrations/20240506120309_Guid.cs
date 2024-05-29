using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApi.Migrations
{
    /// <inheritdoc />
    public partial class Guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
               name: "PK_Station",
               table: "Station");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Station");

            migrationBuilder.AddColumn<Guid>(
                name: "StationId",
                table: "Station",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Station",
                table: "Station",
                column: "StationId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Station",
                table: "Station");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Station");

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Station",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Station",
                table: "Station",
                column: "StationId");
        }
    }
}
