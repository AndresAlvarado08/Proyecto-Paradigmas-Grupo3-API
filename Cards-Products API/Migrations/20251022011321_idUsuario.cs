using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards_Products_API.Migrations
{
    /// <inheritdoc />
    public partial class idUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Id",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "Cards");
        }
    }
}
