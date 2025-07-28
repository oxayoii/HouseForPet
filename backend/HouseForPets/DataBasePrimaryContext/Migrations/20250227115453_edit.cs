using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_userFavorites_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "UserFavoriteId",
                table: "Pets");

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "userFavorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_userFavorites_PetId",
                table: "userFavorites",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_userFavorites_Pets_PetId",
                table: "userFavorites",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userFavorites_Pets_PetId",
                table: "userFavorites");

            migrationBuilder.DropIndex(
                name: "IX_userFavorites_PetId",
                table: "userFavorites");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "userFavorites");

            migrationBuilder.AddColumn<int>(
                name: "UserFavoriteId",
                table: "Pets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_UserFavoriteId",
                table: "Pets",
                column: "UserFavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_userFavorites_UserFavoriteId",
                table: "Pets",
                column: "UserFavoriteId",
                principalTable: "userFavorites",
                principalColumn: "Id");
        }
    }
}
