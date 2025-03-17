using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cart_service.Migrations
{
    /// <inheritdoc />
    public partial class EmailToUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "CartItems",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "CartItems",
                newName: "UserEmail");
        }
    }
}
