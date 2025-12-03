using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignemnt_APDP.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalSlotsToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSlots",
                table: "Enrollments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSlots",
                table: "Enrollments");
        }
    }
}
