using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballWeatherDataConsolidator.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatesLatColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lattitude",
                table: "Stadiums",
                newName: "Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Stadiums",
                newName: "Lattitude");
        }
    }
}
