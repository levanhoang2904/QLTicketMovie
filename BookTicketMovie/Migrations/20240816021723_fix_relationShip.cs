using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTicketMovie.Migrations
{
    /// <inheritdoc />
    public partial class fix_relationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_MovieId",
                table: "Ticket",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Movie_MovieId",
                table: "Ticket",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Movie_MovieId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_MovieId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Ticket");
        }
    }
}
