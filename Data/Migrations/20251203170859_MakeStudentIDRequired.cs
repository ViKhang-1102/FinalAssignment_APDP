using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignemnt_APDP.Migrations
{
    /// <inheritdoc />
    public partial class MakeStudentIDRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete enrollments without StudentID (orphaned data from old schema)
            migrationBuilder.Sql("DELETE FROM Enrollments WHERE StudentID IS NULL");

            // Now make the column non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentID",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
