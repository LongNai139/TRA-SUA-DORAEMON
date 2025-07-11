using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdCustom",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdStaff",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCustom",
                table: "Users",
                column: "IdCustom");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdStaff",
                table: "Users",
                column: "IdStaff");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Custom_IdCustom",
                table: "Users",
                column: "IdCustom",
                principalTable: "Custom",
                principalColumn: "IdCustom",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Staff_IdStaff",
                table: "Users",
                column: "IdStaff",
                principalTable: "Staff",
                principalColumn: "IdStaff",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Custom_IdCustom",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Staff_IdStaff",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdCustom",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdStaff",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdCustom",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IdStaff",
                table: "Users");
        }
    }
}
