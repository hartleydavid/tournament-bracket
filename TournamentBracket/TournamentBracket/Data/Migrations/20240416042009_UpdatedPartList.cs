using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentBracket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPartList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Participants",
                table: "TournamentBrackets");

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_TournamentBrackets_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "TournamentBrackets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participant_TournamentId",
                table: "Participant",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.AddColumn<string>(
                name: "Participants",
                table: "TournamentBrackets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
