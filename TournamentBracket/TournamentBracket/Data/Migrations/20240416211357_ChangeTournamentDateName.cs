using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTournamentDateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "TournamentBrackets",
                newName: "TournamentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentDate",
                table: "TournamentBrackets",
                newName: "CreationDate");
        }
    }
}
