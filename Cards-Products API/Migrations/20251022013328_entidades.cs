using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class entidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Cards_CardId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_CardId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Purchases",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "Purchases",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Purchases",
                newName: "Card_Id");

            migrationBuilder.AddColumn<int>(
                name: "Product_Id",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Card_Id",
                table: "Purchases",
                column: "Card_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Product_Id",
                table: "Purchases",
                column: "Product_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Cards_Card_Id",
                table: "Purchases",
                column: "Card_Id",
                principalTable: "Cards",
                principalColumn: "Card_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_Product_Id",
                table: "Purchases",
                column: "Product_Id",
                principalTable: "Products",
                principalColumn: "Product_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Cards_Card_Id",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_Product_Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Card_Id",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_Product_Id",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Purchases",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Purchases",
                newName: "CardId");

            migrationBuilder.RenameColumn(
                name: "Card_Id",
                table: "Purchases",
                newName: "Amount");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_CardId",
                table: "Purchases",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Cards_CardId",
                table: "Purchases",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Card_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Product_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
