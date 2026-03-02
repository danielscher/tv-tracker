using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTracker.Migrations
{
    /// <inheritdoc />
    public partial class DateColOnMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Movies");

            migrationBuilder.AddColumn<DateTime>(
                name: "MediaInfo_ReleaseDate",
                table: "Series",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Seasons",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MediaInfo_ReleaseDate",
                table: "Movies",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaInfo_ReleaseDate",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "MediaInfo_ReleaseDate",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
