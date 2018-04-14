using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Admins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAdmin",
                value: true);

            migrationBuilder.InsertData(
                table: "UsersSecure",
                columns: new[] { "Id", "IsAdmin", "Name", "Password", "Salt" },
                values: new object[] { 1, true, "adam", "My1VzKf/HYSEfmWCSpXaJc7GhKApd8ZP11waj8CuoSw=", "S3Cr7zOOK6nNqjbnQGUFnA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsersSecure",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAdmin",
                value: false);
        }
    }
}
