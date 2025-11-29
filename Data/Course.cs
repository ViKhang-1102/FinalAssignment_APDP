namespace FinalAssignemnt_APDP.Data
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubjectID { get; set; }

        public Subject Subject { get; set; }

        public int SemesterID { get; set; }

        public Semester Semester { get; set; }

        public string LectureID { get; set; }

        public ApplicationUser Lecture { get; set; }
    }
}
