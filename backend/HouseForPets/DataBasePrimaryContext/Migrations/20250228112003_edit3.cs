using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class edit3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_userFavorites_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_userFavorites_Users_UserId",
                table: "userFavorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userFavorites",
                table: "userFavorites");

            migrationBuilder.DropIndex(
                name: "IX_Pets_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "UserFavoriteId",
                table: "Pets");

            migrationBuilder.RenameTable(
                name: "userFavorites",
                newName: "UserFavorites");

            migrationBuilder.RenameIndex(
                name: "IX_userFavorites_UserId",
                table: "UserFavorites",
                newName: "IX_UserFavorites_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FavoritePetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PetId = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UserFavoriteId = table.Column<int>(type: "integer", nullable: true),
                    CreatAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritePetItems_UserFavorites_UserFavoriteId",
                        column: x => x.UserFavoriteId,
                        principalTable: "UserFavorites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePetItems_UserFavoriteId",
                table: "FavoritePetItems",
                column: "UserFavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Users_UserId",
                table: "UserFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Users_UserId",
                table: "UserFavorites");

            migrationBuilder.DropTable(
                name: "FavoritePetItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavorites",
                table: "UserFavorites");

            migrationBuilder.RenameTable(
                name: "UserFavorites",
                newName: "userFavorites");

            migrationBuilder.RenameIndex(
                name: "IX_UserFavorites_UserId",
                table: "userFavorites",
                newName: "IX_userFavorites_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserFavoriteId",
                table: "Pets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userFavorites",
                table: "userFavorites",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_userFavorites_Users_UserId",
                table: "userFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
