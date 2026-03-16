using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvTracker.Migrations
{
    /// <inheritdoc />
    public partial class UserMediaRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserMedia",
                newName: "Watched");

            migrationBuilder.AddColumn<bool>(
                name: "Saved",
                table: "UserMedia",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saved",
                table: "UserMedia");

            migrationBuilder.RenameColumn(
                name: "Watched",
                table: "UserMedia",
                newName: "Status");
        }
    }
}
