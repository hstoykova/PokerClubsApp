﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerClubsApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlayerGameTableDropId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
