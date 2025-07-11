using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DIHA.Migrations
{
    /// <inheritdoc />
    public partial class insertCustom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Custom",
                columns: table => new
                {
                    IdCustom = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedCustomName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custom", x => x.IdCustom);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Custom");
        }
    }
}
