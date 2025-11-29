namespace FinalAssignemnt_APDP.Data
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        public ApplicationUser Student { get; set; }
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
