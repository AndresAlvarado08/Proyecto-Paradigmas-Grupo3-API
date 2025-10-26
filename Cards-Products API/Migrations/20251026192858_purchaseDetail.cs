using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class purchaseDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_Product_Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Product_Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "Purchases");

            migrationBuilder.CreateTable(
                name: "PurchaseDetails",
                columns: table => new
                {
                    Purchase_Detail_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Purchase_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseDetails", x => x.Purchase_Detail_Id);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Products_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Products",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseDetails_Purchases_Purchase_Id",
                        column: x => x.Purchase_Id,
                        principalTable: "Purchases",
                        principalColumn: "Purchase_Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_Product_Id",
                table: "PurchaseDetails",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseDetails_Purchase_Id",
                table: "PurchaseDetails",
                column: "Purchase_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseDetails");

            migrationBuilder.AddColumn<int>(
                name: "Product_Id",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Product_Id",
                table: "Purchases",
                column: "Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_Product_Id",
                table: "Purchases",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id");
        }
    }
}
