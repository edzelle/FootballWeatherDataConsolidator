using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballWeatherDataConsolidator.Data.Migrations
{
    /// <inheritdoc />
    public partial class changesGameWeatherName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameWeatherEntity_Games_GameId",
                table: "GameWeatherEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameWeatherEntity",
                table: "GameWeatherEntity");

            migrationBuilder.RenameTable(
                name: "GameWeatherEntity",
                newName: "GameWeather");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameWeather",
                table: "GameWeather",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameWeather_Games_GameId",
                table: "GameWeather",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameWeather_Games_GameId",
                table: "GameWeather");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameWeather",
                table: "GameWeather");

            migrationBuilder.RenameTable(
                name: "GameWeather",
                newName: "GameWeatherEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameWeatherEntity",
                table: "GameWeatherEntity",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameWeatherEntity_Games_GameId",
                table: "GameWeatherEntity",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
