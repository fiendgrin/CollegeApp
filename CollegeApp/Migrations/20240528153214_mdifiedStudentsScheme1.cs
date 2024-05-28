using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class mdifiedStudentsScheme1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentsDbTable",
                table: "StudentsDbTable");

            migrationBuilder.RenameTable(
                name: "StudentsDbTable",
                newName: "Student");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "StudentsDbTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentsDbTable",
                table: "StudentsDbTable",
                column: "Id");
        }
    }
}
