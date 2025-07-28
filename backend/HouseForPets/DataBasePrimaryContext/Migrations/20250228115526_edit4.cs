using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class edit4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritePetItems");

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "UserFavorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_PetId",
                table: "UserFavorites",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Pets_PetId",
                table: "UserFavorites",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Pets_PetId",
                table: "UserFavorites");

            migrationBuilder.DropIndex(
                name: "IX_UserFavorites_PetId",
                table: "UserFavorites");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "UserFavorites");

            migrationBuilder.CreateTable(
                name: "FavoritePetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    CreatAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PetId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UserFavoriteId = table.Column<int>(type: "integer", nullable: true)
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
        }
    }
}
