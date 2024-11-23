using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdColumnToPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Players_PlayerAccountId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PlayerAccountId",
                table: "Memberships",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_PlayerAccountId",
                table: "Memberships",
                newName: "IX_Memberships_PlayerId");

            migrationBuilder.DropColumn("AccountId", "Players");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Players_PlayerId",
                table: "Memberships",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Players_PlayerId",
                table: "Memberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Memberships",
                newName: "PlayerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Memberships_PlayerId",
                table: "Memberships",
                newName: "IX_Memberships_PlayerAccountId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Players",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Players_PlayerAccountId",
                table: "Memberships",
                column: "PlayerAccountId",
                principalTable: "Players",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
