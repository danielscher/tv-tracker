using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTracker.Migrations
{
    /// <inheritdoc />
    public partial class ApiIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "UserMedia",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserSeriesId",
                table: "UserMedia",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Series",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Seasons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_UserSeriesId",
                table: "UserMedia",
                column: "UserSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMedia_UserMedia_UserSeriesId",
                table: "UserMedia",
                column: "UserSeriesId",
                principalTable: "UserMedia",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMedia_UserMedia_UserSeriesId",
                table: "UserMedia");

            migrationBuilder.DropIndex(
                name: "IX_UserMedia_UserSeriesId",
                table: "UserMedia");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "UserMedia");

            migrationBuilder.DropColumn(
                name: "UserSeriesId",
                table: "UserMedia");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "Movies");
        }
    }
}
