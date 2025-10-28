using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class purchaseDetailEnPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Purchase_Detail_Id",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Purchase_Detail_Id",
                table: "Purchases",
                column: "Purchase_Detail_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseDetails_Purchase_Detail_Id",
                table: "Purchases",
                column: "Purchase_Detail_Id",
                principalTable: "PurchaseDetails",
                principalColumn: "Purchase_Detail_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseDetails_Purchase_Detail_Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Purchase_Detail_Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Purchase_Detail_Id",
                table: "Purchases");
        }
    }
}
