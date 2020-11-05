using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeAndSell.Data.Migrations
{
    public partial class tradeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Trade",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Trade");
        }
    }
}
