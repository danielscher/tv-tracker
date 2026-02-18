using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTracker.Migrations
{
    /// <inheritdoc />
    public partial class ComplexTypeOnDerived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Genres",
                table: "Series",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Language",
                table: "Series",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_PosterPath",
                table: "Series",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Title",
                table: "Series",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Genres",
                table: "Movies",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Language",
                table: "Movies",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_PosterPath",
                table: "Movies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaInfo_Title",
                table: "Movies",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaInfo_Genres",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MediaInfo_Language",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MediaInfo_PosterPath",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MediaInfo_Title",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "MediaInfo_Genres",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MediaInfo_Language",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MediaInfo_PosterPath",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MediaInfo_Title",
                table: "Movies");
        }
    }
}
