using System;
using System.Collections.Generic;
using System.Linq;
using FinalAssignemnt_APDP.Constants;
using FinalAssignemnt_APDP.Data;

namespace FinalAssignemnt_APDP.Components.Enrollments
{
    public sealed class EnrollmentScheduleState
    {
        private readonly Enrollment _enrollment;
        private string? _startTime;
        private string? _endTime;

        public EnrollmentScheduleState(Enrollment enrollment)
        {
            _enrollment = enrollment ?? throw new ArgumentNullException(nameof(enrollment));
            SyncFromModel();
        }

        public HashSet<DayOfWeek> SelectedDays { get; } = new();

        public string? StartTime
        {
            get => _startTime;
            set
            {
                _startTime = NormalizeTime(value);
                Persist();
            }
        }

        public string? EndTime
        {
            get => _endTime;
            set
            {
                _endTime = NormalizeTime(value);
                Persist();
            }
        }

        public void ToggleDay(DayOfWeek day)
        {
            if (!SelectedDays.Add(day))
            {
                SelectedDays.Remove(day);
            }

            Persist();
        }

        public void SyncFromModel()
        {
            SelectedDays.Clear();
            foreach (var option in WeekdayCatalog.FromStoredValue(_enrollment.DayOfWeek))
            {
                SelectedDays.Add(option.Day);
            }

            if (TimeBlockCatalog.TryGetByLabel(_enrollment.TimeSlot, out var labeledBlock) && labeledBlock is not null)
            {
                _startTime = labeledBlock.Start;
                _endTime = labeledBlock.End;
                return;
            }

            var (start, end) = ParseTimeSlot(_enrollment.TimeSlot);
            _startTime = start;
            _endTime = end;
        }

        public void Persist()
        {
            _enrollment.DayOfWeek = SelectedDays.Count == 0 ? null : WeekdayCatalog.Serialize(SelectedDays);
            _enrollment.TimeSlot = BuildTimeSlotLabel();
        }

        private string? BuildTimeSlotLabel()
        {
            if (string.IsNullOrWhiteSpace(_startTime) && string.IsNullOrWhiteSpace(_endTime))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(_startTime))
            {
                return _endTime;
            }

            if (string.IsNullOrWhiteSpace(_endTime))
            {
                return _startTime;
            }

            if (TimeBlockCatalog.TryGetByRange(_startTime, _endTime, out var block) && block is not null)
            {
                return block.Label;
            }

            return FormattableString.Invariant($"{_startTime} - {_endTime}");
        }

        private static string? NormalizeTime(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

        private static (string? Start, string? End) ParseTimeSlot(string? slot)
        {
            if (string.IsNullOrWhiteSpace(slot))
            {
                return (null, null);
            }

            if (TimeBlockCatalog.TryGetByLabel(slot, out var labeledBlock) && labeledBlock is not null)
            {
                return (labeledBlock.Start, labeledBlock.End);
            }

            var parts = slot.Split('-', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var start = parts.Length > 0 ? parts[0] : null;
            var end = parts.Length > 1 ? parts[1] : null;
            return (start, end);
        }
    }
}
