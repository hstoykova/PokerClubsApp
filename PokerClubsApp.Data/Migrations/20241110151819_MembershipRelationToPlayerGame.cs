using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MembershipRelationToPlayerGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "PlayersGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayersGames_MembershipId",
                table: "PlayersGames",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersGames_Memberships_MembershipId",
                table: "PlayersGames",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersGames_Memberships_MembershipId",
                table: "PlayersGames");

            migrationBuilder.DropIndex(
                name: "IX_PlayersGames_MembershipId",
                table: "PlayersGames");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "PlayersGames");
        }
    }
}
