using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeAndSell.Data.Migrations
{
    public partial class trade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    SellerId = table.Column<string>(nullable: true),
                    BuyerId = table.Column<string>(nullable: true),
                    TradeItemIds = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ApproveStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trade");
        }
    }
}
