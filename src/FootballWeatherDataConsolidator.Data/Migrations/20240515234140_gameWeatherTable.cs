using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballWeatherDataConsolidator.Data.Migrations
{
    /// <inheritdoc />
    public partial class gameWeatherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Stadiums_VenueEntityId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_VenueEntityId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "VenueEntityId",
                table: "Teams");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeam",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GameSite",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeam",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GameWeatherEntity",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageTempurature = table.Column<decimal>(type: "TEXT", nullable: false),
                    AverageRelativeHumitidty = table.Column<decimal>(type: "TEXT", nullable: false),
                    AverageApparentTempurature = table.Column<decimal>(type: "TEXT", nullable: false),
                    AverageRain = table.Column<decimal>(type: "TEXT", nullable: false),
                    AverageSnowfall = table.Column<decimal>(type: "TEXT", nullable: false),
                    AverageWindSpeed = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameWeatherEntity", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_GameWeatherEntity_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameWeatherEntity");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teams",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "VenueEntityId",
                table: "Teams",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HomeTeam",
                table: "Games",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "GameSite",
                table: "Games",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AwayTeam",
                table: "Games",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_VenueEntityId",
                table: "Teams",
                column: "VenueEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Stadiums_VenueEntityId",
                table: "Teams",
                column: "VenueEntityId",
                principalTable: "Stadiums",
                principalColumn: "Id");
        }
    }
}
