using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Custom_IdCustom",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Staff_IdStaff",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "IdStaff",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdCustom",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Custom_IdCustom",
                table: "Users",
                column: "IdCustom",
                principalTable: "Custom",
                principalColumn: "IdCustom");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Staff_IdStaff",
                table: "Users",
                column: "IdStaff",
                principalTable: "Staff",
                principalColumn: "IdStaff");
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

            migrationBuilder.AlterColumn<string>(
                name: "IdStaff",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCustom",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
    }
}
