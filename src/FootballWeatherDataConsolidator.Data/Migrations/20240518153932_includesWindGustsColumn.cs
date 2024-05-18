using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballWeatherDataConsolidator.Data.Migrations
{
    /// <inheritdoc />
    public partial class includesWindGustsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageWindSpeed",
                table: "GameWeather",
                newName: "AverageWindSpeed10M");

            migrationBuilder.RenameColumn(
                name: "AverageTempurature",
                table: "GameWeather",
                newName: "AverageWindGusts10M");

            migrationBuilder.RenameColumn(
                name: "AverageRelativeHumitidty",
                table: "GameWeather",
                newName: "AverageTempurature2M");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageRelativeHumitidty2M",
                table: "GameWeather",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRelativeHumitidty2M",
                table: "GameWeather");

            migrationBuilder.RenameColumn(
                name: "AverageWindSpeed10M",
                table: "GameWeather",
                newName: "AverageWindSpeed");

            migrationBuilder.RenameColumn(
                name: "AverageWindGusts10M",
                table: "GameWeather",
                newName: "AverageTempurature");

            migrationBuilder.RenameColumn(
                name: "AverageTempurature2M",
                table: "GameWeather",
                newName: "AverageRelativeHumitidty");
        }
    }
}
