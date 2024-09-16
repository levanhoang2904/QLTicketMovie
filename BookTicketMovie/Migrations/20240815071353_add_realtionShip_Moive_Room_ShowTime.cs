using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTicketMovie.Migrations
{
    /// <inheritdoc />
    public partial class add_realtionShip_Moive_Room_ShowTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Movie");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_MovieId",
                table: "Showtime",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_RoomId",
                table: "Showtime",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Movie_MovieId",
                table: "Showtime",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Room_RoomId",
                table: "Showtime",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Movie_MovieId",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Room_RoomId",
                table: "Showtime");

            migrationBuilder.DropIndex(
                name: "IX_Showtime_MovieId",
                table: "Showtime");

            migrationBuilder.DropIndex(
                name: "IX_Showtime_RoomId",
                table: "Showtime");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Movie",
                type: "int",
                nullable: true);
        }
    }
}
