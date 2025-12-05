using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FinalAssignemnt_APDP.Constants
{
    public sealed record WeekdayOption(DayOfWeek Day, string Label)
    {
        public string Display => Label;
    }

    public static class WeekdayCatalog
    {
        private static readonly WeekdayOption[] Options =
        [
            new(DayOfWeek.Monday, "Monday"),
            new(DayOfWeek.Tuesday, "Tuesday"),
            new(DayOfWeek.Wednesday, "Wednesday"),
            new(DayOfWeek.Thursday, "Thursday"),
            new(DayOfWeek.Friday, "Friday"),
            new(DayOfWeek.Saturday, "Saturday"),
            new(DayOfWeek.Sunday, "Sunday"),
        ];

        // Common abbreviations mapping (case-insensitive)
        private static readonly IReadOnlyDictionary<string, DayOfWeek> Abbreviations =
            new Dictionary<string, DayOfWeek>(StringComparer.OrdinalIgnoreCase)
            {
                ["mon"] = DayOfWeek.Monday,
                ["monday"] = DayOfWeek.Monday,
                ["tue"] = DayOfWeek.Tuesday,
                ["tues"] = DayOfWeek.Tuesday,
                ["tuesday"] = DayOfWeek.Tuesday,
                ["wed"] = DayOfWeek.Wednesday,
                ["weds"] = DayOfWeek.Wednesday,
                ["wednesday"] = DayOfWeek.Wednesday,
                ["thu"] = DayOfWeek.Thursday,
                ["thur"] = DayOfWeek.Thursday,
                ["thurs"] = DayOfWeek.Thursday,
                ["thursday"] = DayOfWeek.Thursday,
                ["fri"] = DayOfWeek.Friday,
                ["friday"] = DayOfWeek.Friday,
                ["sat"] = DayOfWeek.Saturday,
                ["saturday"] = DayOfWeek.Saturday,
                ["sun"] = DayOfWeek.Sunday,
                ["sunday"] = DayOfWeek.Sunday,
            };

        private static readonly IReadOnlyDictionary<DayOfWeek, WeekdayOption> ByDay = Options.ToDictionary(o => o.Day);
        private static readonly IReadOnlyDictionary<string, WeekdayOption> ByLabel = Options.ToDictionary(o => o.Label, StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyList<WeekdayOption> All => Options;

        public static WeekdayOption? FromDay(DayOfWeek day)
            => ByDay.TryGetValue(day, out var option) ? option : null;

        public static IReadOnlyList<WeekdayOption> FromStoredValue(string? storedValue)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
            {
                return Array.Empty<WeekdayOption>();
            }

            var tokens = storedValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var matches = new List<WeekdayOption>(tokens.Length);

            foreach (var token in tokens)
            {
                if (TryResolveDay(token, out var day) && ByDay.TryGetValue(day, out var option))
                {
                    matches.Add(option);
                }
                else if (ByLabel.TryGetValue(token, out var labelOption))
                {
                    matches.Add(labelOption);
                }
            }

            return matches;
        }

        public static string Serialize(IEnumerable<DayOfWeek> days)
        {
            var ordered = days
                .Distinct()
                .OrderBy(day => (int)day)
                .Select(day => FromDay(day)?.Label ?? day.ToString());

            return string.Join(", ", ordered);
        }

        public static bool TryResolveDay(string? value, out DayOfWeek day)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var trimmed = value.Trim();

                if (TryParseNumericDay(trimmed, out day))
                {
                    return true;
                }

                if (Enum.TryParse(trimmed, true, out day))
                {
                    return true;
                }

                if (ByLabel.TryGetValue(trimmed, out var labelOption))
                {
                    day = labelOption.Day;
                    return true;
                }

                var sanitized = trimmed;
                var parenIndex = trimmed.IndexOf('(');
                if (parenIndex > 0)
                {
                    sanitized = value[..parenIndex].Trim();
                }

                if (TryParseNumericDay(sanitized, out day))
                {
                    return true;
                }

                if (Enum.TryParse(sanitized, true, out day))
                {
                    return true;
                }

                if (ByLabel.TryGetValue(sanitized, out var sanitizedOption))
                {
                    day = sanitizedOption.Day;
                    return true;
                }

                // Support common abbreviations like Mon, Tue, Wed, Thu, Fri, Sat, Sun
                if (!string.IsNullOrWhiteSpace(sanitized))
                {
                    var key = sanitized.Trim();
                    if (Abbreviations.TryGetValue(key, out var abbrDay))
                    {
                        day = abbrDay;
                        return true;
                    }
                    // try first-3-letters fallback
                    if (key.Length >= 3)
                    {
                        var three = key[..3];
                        if (Abbreviations.TryGetValue(three, out var shortDay))
                        {
                            day = shortDay;
                            return true;
                        }
                    }
                }
            }
            day = DayOfWeek.Sunday;
            return false;
        }

        private static bool TryParseNumericDay(string value, out DayOfWeek day)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numeric))
            {
                if (numeric is >= 1 and <= 6)
                {
                    day = (DayOfWeek)numeric;
                    return true;
                }

                if (numeric == 7 || numeric == 0)
                {
                    day = DayOfWeek.Sunday;
                    return true;
                }
            }

            day = DayOfWeek.Sunday;
            return false;
        }
    }
}
