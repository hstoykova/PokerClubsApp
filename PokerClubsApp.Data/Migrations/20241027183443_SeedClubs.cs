using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedClubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.InsertData(
            //    table: "Clubs",
            //    columns: new[] { "Id", "Name", "UnionId" },
            //    values: new object[,]
            //    {
            //        { 1, "GreenLine", 1 },
            //        { 2, "Obi-One", 2 },
            //        { 3, "Galaxy", 3 }
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Clubs",
            //    keyColumn: "Id",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "Clubs",
            //    keyColumn: "Id",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "Clubs",
            //    keyColumn: "Id",
            //    keyValue: 3);
        }
    }
}
