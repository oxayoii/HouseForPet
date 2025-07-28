using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class addtableuserfavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserFavoriteId",
                table: "Pets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "userFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userFavorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_UserFavoriteId",
                table: "Pets",
                column: "UserFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_userFavorites_UserId",
                table: "userFavorites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_userFavorites_UserFavoriteId",
                table: "Pets",
                column: "UserFavoriteId",
                principalTable: "userFavorites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_userFavorites_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "userFavorites");

            migrationBuilder.DropIndex(
                name: "IX_Pets_UserFavoriteId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "UserFavoriteId",
                table: "Pets");
        }
    }
}
