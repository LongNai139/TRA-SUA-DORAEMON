using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class FixHomeAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeAdress",
                table: "Users",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeAdress",
                table: "Users");
        }
    }
}
