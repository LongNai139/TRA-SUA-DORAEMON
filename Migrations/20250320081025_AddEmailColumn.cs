using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "Users",
            type: "nvarchar(256)",
            nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
