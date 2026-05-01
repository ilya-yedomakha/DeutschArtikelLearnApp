using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeutschArtikelLearnApp.Migrations
{
    /// <inheritdoc />
    public partial class langLevelPropertyAssignedToLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "level",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "level",
                table: "Lessons");
        }
    }
}
