using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignemnt_APDP.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Enrollments");
        }
    }
}
