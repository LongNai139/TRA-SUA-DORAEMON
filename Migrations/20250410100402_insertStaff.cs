using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class insertStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    IdStaff = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StaffName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedStaffName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.IdStaff);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
