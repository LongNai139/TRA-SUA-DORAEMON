using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class UppdateHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
;



            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoaiNhanHang",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdCustom",
                table: "HoaDon",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiGiaoHang",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TenKhach",
                table: "HoaDon",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IdCustom",
                table: "HoaDon",
                column: "IdCustom");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_Custom_IdCustom",
                table: "HoaDon",
                column: "IdCustom",
                principalTable: "Custom",
                principalColumn: "IdCustom",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_Custom_IdCustom",
                table: "HoaDon");

            migrationBuilder.DropIndex(
                name: "IX_HoaDon_IdCustom",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "TenKhach",
                table: "HoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoaiNhanHang",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdCustom",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChiGiaoHang",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomIdCustom",
                table: "HoaDon",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_CustomIdCustom",
                table: "HoaDon",
                column: "CustomIdCustom");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_Custom_CustomIdCustom",
                table: "HoaDon",
                column: "CustomIdCustom",
                principalTable: "Custom",
                principalColumn: "IdCustom");
        }
    }
}
