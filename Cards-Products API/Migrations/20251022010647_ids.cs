using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Purchases",
                newName: "Purchase_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "Product_Id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cards",
                newName: "Card_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Purchase_Id",
                table: "Purchases",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Product_Id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Card_Id",
                table: "Cards",
                newName: "Id");
        }
    }
}
