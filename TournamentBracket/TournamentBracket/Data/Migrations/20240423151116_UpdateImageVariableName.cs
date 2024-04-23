using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageVariableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Participants",
                newName: "ImageURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Participants",
                newName: "ImageFileName");
        }
    }
}
