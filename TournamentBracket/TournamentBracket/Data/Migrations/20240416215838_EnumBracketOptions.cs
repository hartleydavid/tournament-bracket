using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnumBracketOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeLosersBracket",
                table: "TournamentBrackets");

            migrationBuilder.AddColumn<int>(
                name: "bracketOptions",
                table: "TournamentBrackets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bracketOptions",
                table: "TournamentBrackets");

            migrationBuilder.AddColumn<bool>(
                name: "IncludeLosersBracket",
                table: "TournamentBrackets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
