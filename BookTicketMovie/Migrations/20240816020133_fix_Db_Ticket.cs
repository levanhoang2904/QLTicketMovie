using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTicketMovie.Migrations
{
    /// <inheritdoc />
    public partial class fix_Db_Ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Ticket");

            migrationBuilder.AddColumn<string>(
                name: "ChairId",
                table: "Ticket",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ChairId",
                table: "Ticket",
                column: "ChairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Chair_ChairId",
                table: "Ticket",
                column: "ChairId",
                principalTable: "Chair",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Chair_ChairId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_ChairId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ChairId",
                table: "Ticket");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Ticket",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
