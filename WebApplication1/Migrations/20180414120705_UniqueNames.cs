using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class UniqueNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UsersSecure",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UsersNotSercure",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.CreateIndex(
                name: "IX_UsersSecure_Name",
                table: "UsersSecure",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersNotSercure_Name",
                table: "UsersNotSercure",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsersSecure_Name",
                table: "UsersSecure");

            migrationBuilder.DropIndex(
                name: "IX_UsersNotSercure_Name",
                table: "UsersNotSercure");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UsersSecure",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UsersNotSercure",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);
        }
    }
}
