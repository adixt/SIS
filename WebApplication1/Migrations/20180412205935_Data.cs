using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UsersNotSercure",
                columns: new[] { "Id", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "adam", "test" },
                    { 2, "rafal", "test2" },
                    { 3, "ewa", "test3" },
                    { 4, "asia", "test4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UsersNotSercure",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
