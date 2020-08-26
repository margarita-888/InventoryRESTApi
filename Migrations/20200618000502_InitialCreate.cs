using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryRESTApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    DeliveryPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InventoryItemId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(23)", maxLength: 23, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItemOption_InventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemOption_InventoryItemId",
                table: "InventoryItemOption",
                column: "InventoryItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemOption");

            migrationBuilder.DropTable(
                name: "InventoryItem");
        }
    }
}