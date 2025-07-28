using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBaseContext.Migrations
{
    /// <inheritdoc />
    public partial class updatetablepermissionrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatAt",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatAt",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "Permissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatAt",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "Permissions");
        }
    }
}
