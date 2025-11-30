using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignemnt_APDP.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGradeLogicToPMD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cập nhật lại LetterGrade và IsPassed cho tất cả các bản ghi hiện có
            // theo logic mới: D (>=80), M (65-79), P (50-64), F (<50)
            
            // Cập nhật LetterGrade = 'P' cho điểm từ 50-64 (trước đây là 'F')
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET LetterGrade = 'P', IsPassed = 1
                WHERE AverageScore >= 50 AND AverageScore < 65
            ");
            
            // Cập nhật IsPassed = 0 cho điểm < 50 (F)
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET IsPassed = 0
                WHERE AverageScore < 50
            ");
            
            // Đảm bảo các điểm >= 50 đều có IsPassed = 1
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET IsPassed = 1
                WHERE AverageScore >= 50
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback về logic cũ: chỉ có D, M, F (không có P)
            // Điểm 50-64 sẽ quay về F và IsPassed = 0
            
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET LetterGrade = 'F', IsPassed = 0
                WHERE AverageScore >= 50 AND AverageScore < 65
            ");
            
            // Cập nhật IsPassed = 0 cho điểm < 65
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET IsPassed = 0
                WHERE AverageScore < 65
            ");
            
            // Đảm bảo các điểm >= 65 đều có IsPassed = 1
            migrationBuilder.Sql(@"
                UPDATE Grades 
                SET IsPassed = 1
                WHERE AverageScore >= 65
            ");
        }
    }
}
