using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class UsuariosQuemados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_User_Id1",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_User_Id1",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "User_Id1",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Id1",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_User_Id1",
                table: "Cards",
                column: "User_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Users_User_Id1",
                table: "Cards",
                column: "User_Id1",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
