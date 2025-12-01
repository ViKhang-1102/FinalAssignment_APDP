namespace FinalAssignemnt_APDP.Data
{
    public class Grade
    {
        public int Id { get; set; }
        
        public string StudentID { get; set; }
        public ApplicationUser Student { get; set; }
        
        public int CourseID { get; set; }
        public Course Course { get; set; }
        
        public double? MidtermScore { get; set; }
        
        public double? FinalScore { get; set; }
        
        public double? AverageScore { get; set; }
        
        public string? LetterGrade { get; set; }
        
        public bool IsPassed { get; set; }
        
        public string? Note { get; set; }
        
        public void CalculateGrade()
        {
            if (MidtermScore.HasValue && FinalScore.HasValue)
            {
                if (!ValidateScore(MidtermScore.Value) || !ValidateScore(FinalScore.Value))
                {
                    return;
                }
                
                AverageScore = (MidtermScore.Value * 0.4) + (FinalScore.Value * 0.6);
                LetterGrade = GetLetterGrade(AverageScore.Value);
                IsPassed = AverageScore.Value >= 50;
            }
        }
        
        private bool ValidateScore(double score)
        {
            return score >= 0 && score <= 100;
        }
        
        private string GetLetterGrade(double score)
        {
            if (score >= 80) return "D";
            if (score >= 65) return "M";
            if (score >= 50) return "P";
            return "F";
        }
    }
}
