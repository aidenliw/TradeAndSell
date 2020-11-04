using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeAndSell.Data.Migrations
{
    public partial class items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Condition = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    AllowTrade = table.Column<string>(nullable: true),
                    MinTradePrice = table.Column<float>(nullable: false),
                    MaxTradePrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
