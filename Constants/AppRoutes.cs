using System;

namespace FinalAssignemnt_APDP.Constants
{
    /// <summary>
    /// Centralized route constants for the application
    /// </summary>
    public static class AppRoutes
    {
        // Auth Routes
        public const string Auth = "/auth";
        public const string Login = "/Account/Login";
        public const string Register = "/Account/Register";
        public const string Logout = "/Account/Logout";
        public const string ManageAccount = "/Account/Manage";
        public const string Profile = "/profile";
        public const string ProfileForgotPassword = "/profile/forgot-password";

        // Admin & Lecturer Routes
        public const string Home = "/";
        public const string LecturerWorkspace = "/lecturer";
        public const string LecturerClasses = "/lecturer/classes";
        public const string LecturerCourseRosterPage = "/lecturer/course-classes";
        public const string LecturerCourseInfoPage = "/lecturer/course-info";
        public const string LecturerTimetable = "/lecturer/timetable";
        public const string LecturerStudents = "/lecturer/students";
        public const string Departments = "/departments";
        public const string DepartmentsCreate = "/departments/create";
        public const string Courses = "/courses";
        public const string CoursesCreate = "/courses/create";
        public const string Semesters = "/semesters";
        public const string SemestersCreate = "/semesters/create";
        public const string Majors = "/majors";
        public const string MajorsCreate = "/majors/create";
        public const string Subjects = "/subjects";
        public const string SubjectsCreate = "/subjects/create";
        public const string Enrollments = "/enrollments";
        public const string EnrollmentsCreate = "/enrollments/create";
        public const string EnrollmentsAssignStudents = "/enrollments/assign-students";
        public const string Users = "/users";
        public const string UsersCreate = "/users/create";
        public const string AdminDashboard = "/admin";
        public const string AdminGrades = "/admin/grades";
        public const string AdminGradesImport = "/admin/grades/import";

        // Student Routes
        public const string StudentDashboard = "/student/dashboard";
        public const string StudentSchedule = "/student/schedule";
        public const string StudentGrades = "/student/grades";
        public const string StudentCourses = "/student/courses";
        public const string StudentProfile = "/student/profile";

        // Helper methods for dynamic routes with parameters
        
        // Department Routes
        public static string DepartmentsEdit(int id) => $"/departments/edit?id={id}";
        public static string DepartmentsDetails(int id) => $"/departments/details?id={id}";
        public static string DepartmentsDelete(int id) => $"/departments/delete?id={id}";

        // Course Routes
        public static string CoursesEdit(int id) => $"/courses/edit?id={id}";
        public static string CoursesDetails(int id) => $"/courses/details?id={id}";
        public static string CoursesDelete(int id) => $"/courses/delete?id={id}";

        // Semester Routes
        public static string SemestersEdit(int id) => $"/semesters/edit?id={id}";
        public static string SemestersDetails(int id) => $"/semesters/details?id={id}";
        public static string SemestersDelete(int id) => $"/semesters/delete?id={id}";

        // Major Routes
        public static string MajorsEdit(int id) => $"/majors/edit?id={id}";
        public static string MajorsDetails(int id) => $"/majors/details?id={id}";
        public static string MajorsDelete(int id) => $"/majors/delete?id={id}";

        // Subject Routes
        public static string SubjectsEdit(int id) => $"/subjects/edit?id={id}";
        public static string SubjectsDetails(int id) => $"/subjects/details?id={id}";
        public static string SubjectsDelete(int id) => $"/subjects/delete?id={id}";

        // Enrollment Routes
        public static string EnrollmentsEdit(int id) => $"/enrollments/edit?id={id}";
        public static string EnrollmentsDetails(int id) => $"/enrollments/details?id={id}";
        public static string EnrollmentsDelete(int id) => $"/enrollments/delete?id={id}";

        // User Routes
        public static string UsersEdit(string id) => $"/users/edit?id={id}";
        public static string UsersDetails(string id) => $"/users/details?id={id}";
        public static string UsersDelete(string id) => $"/users/delete?id={id}";

        // Grade Routes
        public static string AdminGradesDelete(int id) => $"/admin/grades/delete?id={id}";

        // Lecturer course detail routes
        public static string LecturerCourseClasses(int courseId) => $"{LecturerCourseRosterPage}?id={courseId}";
        public static string LecturerCourseInfo(int courseId) => $"{LecturerCourseInfoPage}?id={courseId}";

        // Login with return URL
        public static string LoginWithReturnUrl(string returnUrl) => 
            $"{Login}?returnUrl={Uri.EscapeDataString(returnUrl)}";
    }
}
