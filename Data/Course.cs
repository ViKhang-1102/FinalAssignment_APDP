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

        // Timetable information
        public string? DayOfWeek { get; set; } // Monday, Tusday, ...
        public string? TimeSlot { get; set; }  // Morning, Afternoon, Evening
        public string? Room { get; set; }       // Room number or location
    }
}
