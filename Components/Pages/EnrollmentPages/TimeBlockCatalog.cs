using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalAssignemnt_APDP.Components.Enrollments
{
    public sealed record TimeBlockOption(string Start, string End, string Label);

    public static class TimeBlockCatalog
    {
        private static readonly TimeBlockOption[] Options =
        [
            new("07:00", "11:00", "7 AM - 11 AM"),
            new("12:00", "16:00", "12 PM - 4 PM"),
        ];

        private static readonly IReadOnlyDictionary<string, TimeBlockOption> ByStart = Options.ToDictionary(option => option.Start, StringComparer.OrdinalIgnoreCase);
        private static readonly IReadOnlyDictionary<string, TimeBlockOption> ByLabel = Options.ToDictionary(option => option.Label, StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyList<TimeBlockOption> All => Options;

        public static bool TryGetByStart(string? start, out TimeBlockOption? option)
        {
            if (!string.IsNullOrWhiteSpace(start) && ByStart.TryGetValue(start, out option))
            {
                return true;
            }

            option = null;
            return false;
        }

        public static bool TryGetByLabel(string? label, out TimeBlockOption? option)
        {
            if (!string.IsNullOrWhiteSpace(label) && ByLabel.TryGetValue(label.Trim(), out option))
            {
                return true;
            }

            option = null;
            return false;
        }

        public static bool TryGetByRange(string? start, string? end, out TimeBlockOption? option)
        {
            if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
            {
                option = Options.FirstOrDefault(block =>
                    string.Equals(block.Start, start, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(block.End, end, StringComparison.OrdinalIgnoreCase));

                if (option is not null)
                {
                    return true;
                }
            }

            option = null;
            return false;
        }
    }
}
