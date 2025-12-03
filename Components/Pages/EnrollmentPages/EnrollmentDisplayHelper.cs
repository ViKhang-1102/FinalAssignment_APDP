using System;
using System.Collections.Generic;
using System.Linq;
using FinalAssignemnt_APDP.Constants;
using FinalAssignemnt_APDP.Data;

namespace FinalAssignemnt_APDP.Components.Enrollments
{
    public static class EnrollmentDisplayHelper
    {
        public static string FormatStudentOption(ApplicationUser student)
        {
            var parts = BuildStudentParts(student);
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                parts.Add(student.Email!);
            }

            return string.Join(" - ", parts);
        }

        public static string FormatStudent(ApplicationUser? student, string? fallbackId)
        {
            if (student is null)
            {
                return fallbackId ?? string.Empty;
            }

            var parts = BuildStudentParts(student);
            if (!string.IsNullOrWhiteSpace(student.Email))
            {
                parts.Add(student.Email!);
            }

            return parts.Count == 0 ? fallbackId ?? string.Empty : string.Join(" - ", parts);
        }

        public static string FormatCourseOption(Course course)
        {
            var parts = BuildCourseParts(course);
            parts.Add(FormattableString.Invariant($"#{course.Id}"));
            return string.Join(" | ", parts);
        }

        public static string FormatCourse(Course? course, int? fallbackId = null)
        {
            if (course is null)
            {
                return fallbackId is null ? string.Empty : FormattableString.Invariant($"Course #{fallbackId}");
            }

            var parts = BuildCourseParts(course);
            parts.Add(FormattableString.Invariant($"#{course.Id}"));
            return string.Join(" | ", parts);
        }

        public static string FormatDayPattern(string? dayPattern)
        {
            var options = WeekdayCatalog.FromStoredValue(dayPattern);
            if (options.Count == 0)
            {
                return string.IsNullOrWhiteSpace(dayPattern) ? "—" : dayPattern!;
            }

            return string.Join(", ", options.Select(option => option.Label));
        }

        public static string GetDayChipClasses(bool isSelected)
            => isSelected
                ? "inline-flex flex-col rounded-xl border border-indigo-200 bg-white px-3 py-2 text-indigo-600 shadow-sm"
                : "inline-flex flex-col rounded-xl border border-slate-200 bg-white px-3 py-2 text-slate-500 hover:border-slate-300";

        private static List<string> BuildStudentParts(ApplicationUser student)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(student.StudentId))
            {
                parts.Add(student.StudentId!);
            }

            if (!string.IsNullOrWhiteSpace(student.Name))
            {
                parts.Add(student.Name!);
            }

            return parts;
        }

        private static List<string> BuildCourseParts(Course course)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(course.Name))
            {
                parts.Add(course.Name!);
            }

            if (!string.IsNullOrWhiteSpace(course.Subject?.Name))
            {
                parts.Add(course.Subject!.Name);
            }

            if (!string.IsNullOrWhiteSpace(course.Semester?.Name))
            {
                parts.Add(course.Semester!.Name);
            }

            return parts;
        }
    }
}
