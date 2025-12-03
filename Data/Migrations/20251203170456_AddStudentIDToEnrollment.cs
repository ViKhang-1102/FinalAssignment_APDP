using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignemnt_APDP.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentIDToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, add the column as nullable to avoid constraint conflicts
            migrationBuilder.AddColumn<string>(
                name: "StudentID",
                table: "Enrollments",
                type: "nvarchar(450)",
                nullable: true);

            // If you have existing data, you might need to update StudentID values here
            // migrationBuilder.Sql("UPDATE Enrollments SET StudentID = ... WHERE ...");

            // Now alter the column to be non-nullable (only if all rows have valid StudentID)
            // Uncomment the following lines after populating StudentID for existing rows:
            // migrationBuilder.AlterColumn<string>(
            //     name: "StudentID",
            //     table: "Enrollments",
            //     type: "nvarchar(450)",
            //     nullable: false,
            //     oldClrType: typeof(string),
            //     oldType: "nvarchar(450)",
            //     oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentID",
                table: "Enrollments",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentID",
                table: "Enrollments",
                column: "StudentID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_StudentID",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentID",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Enrollments");
        }
    }
}
