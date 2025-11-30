namespace FinalAssignemnt_APDP.Data
{
    public class Grade
    {
        public int Id { get; set; }
        
        public string StudentID { get; set; }
        public ApplicationUser Student { get; set; }
        
        public int CourseID { get; set; }
        public Course Course { get; set; }
        
        // ?i?m gi?a k? (40%) - Thang ?i?m 0-100
        public double? MidtermScore { get; set; }
        
        // ?i?m cu?i k? (60%) - Thang ?i?m 0-100
        public double? FinalScore { get; set; }
        
        // ?i?m trung bình - Thang ?i?m 0-100
        public double? AverageScore { get; set; }
        
        // ?i?m ch? (D, M, P, F)
        public string? LetterGrade { get; set; }
        
        // Tr?ng thái ??u/r?t
        public bool IsPassed { get; set; }
        
        // Ghi chú
        public string? Note { get; set; }
        
        // T? ??ng tính ?i?m trung bình và ?i?m ch?
        public void CalculateGrade()
        {
            if (MidtermScore.HasValue && FinalScore.HasValue)
            {
                // Validate ?i?m trong kho?ng 0-100
                if (!ValidateScore(MidtermScore.Value) || !ValidateScore(FinalScore.Value))
                {
                    return;
                }
                
                AverageScore = (MidtermScore.Value * 0.4) + (FinalScore.Value * 0.6);
                LetterGrade = GetLetterGrade(AverageScore.Value);
                IsPassed = AverageScore.Value >= 50;
            }
        }
        
        // Ki?m tra ?i?m h?p l? (0-100)
        private bool ValidateScore(double score)
        {
            return score >= 0 && score <= 100;
        }
        
        private string GetLetterGrade(double score)
        {
            // ?i?m >= 80: D (Distinction - Gi?i)
            if (score >= 80) return "D";
            // ?i?m 65-79: M (Merit - Khá)
            if (score >= 65) return "M";
            // ?i?m 50-64: P (Pass - ??t)
            if (score >= 50) return "P";
            // ?i?m < 50: F (Fail - Không ??t)
            return "F";
        }
    }
}
