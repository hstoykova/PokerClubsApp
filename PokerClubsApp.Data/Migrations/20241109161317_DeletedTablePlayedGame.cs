using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletedTablePlayedGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersGames_PlayedGames_GameId",
                table: "PlayersGames");

            migrationBuilder.DropTable(
                name: "PlayedGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersGames",
                table: "PlayersGames");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "PlayersGames",
                newName: "GameTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayersGames_GameId",
                table: "PlayersGames",
                newName: "IX_PlayersGames_GameTypeId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlayersGames",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "PlayersGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "PlayersGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersGames",
                table: "PlayersGames",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersGames_PlayerAccountId",
                table: "PlayersGames",
                column: "PlayerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersGames_GamesTypes_GameTypeId",
                table: "PlayersGames",
                column: "GameTypeId",
                principalTable: "GamesTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersGames_GamesTypes_GameTypeId",
                table: "PlayersGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersGames",
                table: "PlayersGames");

            migrationBuilder.DropIndex(
                name: "IX_PlayersGames_PlayerAccountId",
                table: "PlayersGames");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayersGames");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "PlayersGames");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "PlayersGames");

            migrationBuilder.RenameColumn(
                name: "GameTypeId",
                table: "PlayersGames",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayersGames_GameTypeId",
                table: "PlayersGames",
                newName: "IX_PlayersGames_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersGames",
                table: "PlayersGames",
                columns: new[] { "PlayerAccountId", "GameId" });

            migrationBuilder.CreateTable(
                name: "PlayedGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameTypeId = table.Column<int>(type: "int", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayedGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayedGames_GamesTypes_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GamesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayedGames_GameTypeId",
                table: "PlayedGames",
                column: "GameTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersGames_PlayedGames_GameId",
                table: "PlayersGames",
                column: "GameId",
                principalTable: "PlayedGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
