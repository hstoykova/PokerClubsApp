﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPlayerGamePlayersRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersGames_Players_PlayerAccountId",
                table: "PlayersGames");

            migrationBuilder.DropIndex(
                name: "IX_PlayersGames_PlayerAccountId",
                table: "PlayersGames");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_ClubId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "PlayerAccountId",
                table: "PlayersGames");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_Club_PlayerAccountId",
                table: "Memberships",
                columns: new[] { "ClubId", "PlayerAccountId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memberships_Club_PlayerAccountId",
                table: "Memberships");

            migrationBuilder.AddColumn<int>(
                name: "PlayerAccountId",
                table: "PlayersGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayersGames_PlayerAccountId",
                table: "PlayersGames",
                column: "PlayerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_ClubId",
                table: "Memberships",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersGames_Players_PlayerAccountId",
                table: "PlayersGames",
                column: "PlayerAccountId",
                principalTable: "Players",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
