using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SneakersShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUseCases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserUseCases",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UseCaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUseCases", x => new { x.RoleId, x.UseCaseId });
                    table.ForeignKey(
                        name: "FK_UserUseCases_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUseCases");
        }
    }
}
