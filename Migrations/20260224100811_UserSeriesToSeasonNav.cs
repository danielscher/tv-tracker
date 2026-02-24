using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTracker.Migrations
{
    /// <inheritdoc />
    public partial class UserSeriesToSeasonNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserSeriesId",
                table: "UserMedia",
                type: "TEXT",
                nullable: true);

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
                name: "UserSeriesId",
                table: "UserMedia");
        }
    }
}
