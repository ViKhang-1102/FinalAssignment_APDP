namespace FinalAssignemnt_APDP.Data
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        public ApplicationUser Student { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
        public string? DayOfWeek { get; set; }
        public string? Room { get; set; }
        public string? TimeSlot { get; set; }
        public int? TotalSlots { get; set; }
    }
}
