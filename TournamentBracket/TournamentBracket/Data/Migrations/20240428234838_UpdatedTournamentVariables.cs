using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTournamentVariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentDate",
                table: "TournamentBrackets");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TournamentBrackets",
                newName: "UserCreatedID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCreatedID",
                table: "TournamentBrackets",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "TournamentDate",
                table: "TournamentBrackets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
