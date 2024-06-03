using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedMatchesClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoundNumber = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    Winner = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }
    }
}
