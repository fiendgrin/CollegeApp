using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToStudentstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StudentsDbTable",
                columns: new[] { "Id", "Address", "DOB", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Baku", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "yunis@mail.ru", "Yunis" },
                    { 2, "Dubai", new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "ali@mail.ru", "Ali" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StudentsDbTable",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StudentsDbTable",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
