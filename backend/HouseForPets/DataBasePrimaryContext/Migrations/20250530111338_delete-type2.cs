using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class deletetype2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Pets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Pets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
