using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FinalAssignemnt_APDP.Constants;
using FinalAssignemnt_APDP.Data;
using Microsoft.EntityFrameworkCore;

namespace FinalAssignemnt_APDP.Services
{
    public class LecturerWorkspaceService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public LecturerWorkspaceService(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<LecturerWorkspaceResult> LoadAsync(string lecturerId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(lecturerId))
            {
                throw new ArgumentException("Lecturer identifier is required.", nameof(lecturerId));
            }

            await using var context = await _dbFactory.CreateDbContextAsync(cancellationToken);

            var courses = await context.Courses
                .AsNoTracking()
                .Include(c => c.Subject)
                .Include(c => c.Semester)
                .Where(c => c.LectureID == lecturerId)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            if (courses.Count == 0)
            {
                return LecturerWorkspaceResult.Empty;
            }

            var courseIds = courses.Select(c => c.Id).ToList();

            var enrollments = await context.Enrollments
                .AsNoTracking()
                .Include(e => e.Student)
                .Where(e => courseIds.Contains(e.CourseID))
                .ToListAsync(cancellationToken);

            var grades = await context.Grades
                .AsNoTracking()
                .Include(g => g.Student)
                .Where(g => courseIds.Contains(g.CourseID))
                .ToListAsync(cancellationToken);

            var gradeLookup = grades
                .Where(g => !string.IsNullOrWhiteSpace(g.StudentID))
                .ToDictionary(
                    g => (g.CourseID, NormalizeKey(g.StudentID!)),
                    g => g,
                    CourseStudentComparer.Instance);

            var distinctStudents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var courseMeetings = new Dictionary<int, IReadOnlyList<LecturerClassMeeting>>();
            var courseRosters = new Dictionary<int, IReadOnlyList<LecturerRosterRow>>();
            var courseOverview = new List<LecturerCourseOverview>(courses.Count);

            foreach (var course in courses)
            {
                var courseEnrollments = enrollments.Where(e => e.CourseID == course.Id).ToList();
                var meetings = BuildClassMeetings(courseEnrollments);
                courseMeetings[course.Id] = meetings;

                var roster = BuildRoster(course.Id, courseEnrollments, gradeLookup, distinctStudents);
                courseRosters[course.Id] = roster;
                var totalSlots = courseEnrollments
                    .Select(e => e.TotalSlots)
                    .Where(slot => slot.HasValue)
                    .DefaultIfEmpty()
                    .Max();

                courseOverview.Add(new LecturerCourseOverview
                {
                    Id = course.Id,
                    Name = course.Name,
                    Subject = course.Subject?.Name ?? "-",
                    Semester = course.Semester?.Name ?? "-",
                    StudentCount = roster.Count,
                    TotalSlots = totalSlots,
                    ScheduleSummary = BuildScheduleSummary(courseEnrollments)
                });
            }

            var upcomingSessions = BuildUpcomingSessions(courses, courseMeetings);

            return new LecturerWorkspaceResult
            {
                Courses = courseOverview,
                CourseMeetings = courseMeetings,
                CourseRosters = courseRosters,
                UpcomingSessions = upcomingSessions,
                TotalStudents = distinctStudents.Count
            };
        }

        private static List<LecturerClassMeeting> BuildClassMeetings(IEnumerable<Enrollment> enrollments)
        {
            var meetings = new List<LecturerClassMeeting>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var enrollment in enrollments)
            {
                var time = string.IsNullOrWhiteSpace(enrollment.TimeSlot) ? "TBD" : enrollment.TimeSlot!;
                var room = string.IsNullOrWhiteSpace(enrollment.Room) ? "TBD" : enrollment.Room!;
                var dayOptions = WeekdayCatalog.FromStoredValue(enrollment.DayOfWeek);

                if (dayOptions.Count == 0)
                {
                    AddMeeting("TBD", 7);
                }
                else
                {
                    foreach (var option in dayOptions)
                    {
                        AddMeeting(option.Display, (int)option.Day);
                    }
                }

                void AddMeeting(string dayLabel, int sortOrder)
                {
                    var key = string.Join('|', dayLabel, time, room);
                    if (seen.Add(key))
                    {
                        meetings.Add(new LecturerClassMeeting
                        {
                            Day = dayLabel,
                            Time = time,
                            Room = room,
                            SortOrder = sortOrder
                        });
                    }
                }
            }

            return meetings
                .OrderBy(m => m.SortOrder)
                .ThenBy(m => m.Time, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static List<LecturerRosterRow> BuildRoster(
            int courseId,
            IEnumerable<Enrollment> enrollments,
            IReadOnlyDictionary<(int CourseId, string StudentId), Grade> gradeLookup,
            HashSet<string> distinctStudents)
        {
            var roster = new List<LecturerRosterRow>();
            var grouped = enrollments
                .Where(e => !string.IsNullOrWhiteSpace(e.StudentID))
                .GroupBy(e => NormalizeKey(e.StudentID!), StringComparer.OrdinalIgnoreCase);

            foreach (var group in grouped)
            {
                var enrollment = group.First();
                distinctStudents.Add(group.Key);
                var student = enrollment.Student;

                gradeLookup.TryGetValue((courseId, group.Key), out var grade);

                roster.Add(new LecturerRosterRow
                {
                    StudentId = group.Key,
                    Name = string.IsNullOrWhiteSpace(student?.Name) ? (student?.Email ?? group.Key) : student!.Name!,
                    Email = student?.Email ?? "-",
                    MidtermScore = grade?.MidtermScore,
                    FinalScore = grade?.FinalScore,
                    AverageScore = grade?.AverageScore,
                    LetterGrade = grade?.LetterGrade ?? "-"
                });
            }

            return roster
                .OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static IReadOnlyList<LecturerSessionSnapshot> BuildUpcomingSessions(List<Course> courses, IReadOnlyDictionary<int, IReadOnlyList<LecturerClassMeeting>> courseMeetings)
        {
            var sessions = new List<(LecturerSessionSnapshot Snapshot, int SortOrder)>();

            foreach (var course in courses)
            {
                if (!courseMeetings.TryGetValue(course.Id, out var meetings))
                {
                    continue;
                }

                foreach (var meeting in meetings)
                {
                    sessions.Add((new LecturerSessionSnapshot
                    {
                        CourseId = course.Id,
                        CourseName = course.Name,
                        DayOfWeek = meeting.Day,
                        TimeLabel = meeting.Time,
                        Room = meeting.Room
                    }, meeting.SortOrder));
                }
            }

            return sessions
                .OrderBy(s => s.SortOrder)
                .ThenBy(s => s.Snapshot.TimeLabel, StringComparer.OrdinalIgnoreCase)
                .ThenBy(s => s.Snapshot.CourseName, StringComparer.OrdinalIgnoreCase)
                .Select(s => s.Snapshot)
                .ToList();
        }

        private static string BuildScheduleSummary(IEnumerable<Enrollment> courseEnrollments)
        {
            var slotLabels = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var enrollment in courseEnrollments)
            {
                var time = string.IsNullOrWhiteSpace(enrollment.TimeSlot) ? string.Empty : enrollment.TimeSlot!;
                var dayOptions = WeekdayCatalog.FromStoredValue(enrollment.DayOfWeek);

                if (dayOptions.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(time))
                    {
                        AddLabel($"TBD {time}");
                    }
                    continue;
                }

                foreach (var option in dayOptions)
                {
                    var label = string.IsNullOrWhiteSpace(time) ? option.Display : FormattableString.Invariant($"{option.Display} {time}");
                    AddLabel(label);
                }
            }

            if (slotLabels.Count > 0)
            {
                return string.Join(", ", slotLabels.Take(3));
            }

            var dayOnly = new List<string>();
            var daySeen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var option in courseEnrollments.SelectMany(e => WeekdayCatalog.FromStoredValue(e.DayOfWeek)))
            {
                if (daySeen.Add(option.Display))
                {
                    dayOnly.Add(option.Display);
                }
            }

            return dayOnly.Count > 0 ? string.Join(", ", dayOnly) : "Schedule pending";

            void AddLabel(string value)
            {
                if (seen.Add(value))
                {
                    slotLabels.Add(value);
                }
            }
        }

        private static string NormalizeKey(string value) => value.Trim();

        private sealed class CourseStudentComparer : IEqualityComparer<(int CourseId, string StudentId)>
        {
            public static CourseStudentComparer Instance { get; } = new();

            public bool Equals((int CourseId, string StudentId) x, (int CourseId, string StudentId) y)
                => x.CourseId == y.CourseId && string.Equals(x.StudentId, y.StudentId, StringComparison.OrdinalIgnoreCase);

            public int GetHashCode((int CourseId, string StudentId) obj)
                => HashCode.Combine(obj.CourseId, obj.StudentId?.ToUpperInvariant());
        }
    }

    public sealed class LecturerWorkspaceResult
    {
        public static LecturerWorkspaceResult Empty => new();

        public IReadOnlyList<LecturerCourseOverview> Courses { get; init; } = Array.Empty<LecturerCourseOverview>();
        public IReadOnlyDictionary<int, IReadOnlyList<LecturerClassMeeting>> CourseMeetings { get; init; } = new Dictionary<int, IReadOnlyList<LecturerClassMeeting>>();
        public IReadOnlyDictionary<int, IReadOnlyList<LecturerRosterRow>> CourseRosters { get; init; } = new Dictionary<int, IReadOnlyList<LecturerRosterRow>>();
        public IReadOnlyList<LecturerSessionSnapshot> UpcomingSessions { get; init; } = Array.Empty<LecturerSessionSnapshot>();
        public int TotalStudents { get; init; }

        public bool HasCourses => Courses.Count > 0;
    }

    public sealed class LecturerCourseOverview
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Subject { get; init; } = string.Empty;
        public string Semester { get; init; } = string.Empty;
        public int StudentCount { get; init; }
        public int? TotalSlots { get; init; }
        public string ScheduleSummary { get; init; } = "Schedule pending";
    }

    public sealed class LecturerClassMeeting
    {
        public string Day { get; init; } = "TBD";
        public string Time { get; init; } = "TBD";
        public string Room { get; init; } = "TBD";
        public int SortOrder { get; init; } = 7;
    }

    public sealed class LecturerRosterRow
    {
        public string StudentId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public double? MidtermScore { get; init; }
        public double? FinalScore { get; init; }
        public double? AverageScore { get; init; }
        public string LetterGrade { get; init; } = "-";
    }

    public sealed class LecturerSessionSnapshot
    {
        public int? CourseId { get; init; }
        public string CourseName { get; init; } = string.Empty;
        public string DayOfWeek { get; init; } = "TBD";
        public string TimeLabel { get; init; } = "TBD";
        public string Room { get; init; } = "TBD";
    }
}
