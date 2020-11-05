using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeAndSell.Data.Migrations
{
    public partial class message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<string>(nullable: true),
                    SenderId = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Datetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
