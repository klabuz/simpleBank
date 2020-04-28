using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleBank.Migrations
{
    public partial class StockUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stocks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
