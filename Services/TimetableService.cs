using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace FinalAssignemnt_APDP.Services
{
    public class TimetableService
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _gate = new(1, 1);
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public TimetableService(IWebHostEnvironment env)
        {
            var dataFolder = Path.Combine(env.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataFolder);
            _filePath = Path.Combine(dataFolder, "timetable.json");
        }

        public async Task<IReadOnlyList<TimetableSlot>> GetAllAsync()
        {
            await _gate.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    return Array.Empty<TimetableSlot>();
                }

                var json = await File.ReadAllTextAsync(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return Array.Empty<TimetableSlot>();
                }

                return JsonSerializer.Deserialize<List<TimetableSlot>>(json, _options) ?? new List<TimetableSlot>();
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<TimetableSlot?> GetAsync(Guid id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(slot => slot.Id == id);
        }

        public async Task SaveAsync(TimetableSlot slot)
        {
            if (slot.Id == Guid.Empty)
            {
                slot.Id = Guid.NewGuid();
            }

            await _gate.WaitAsync();
            try
            {
                var slots = (await ReadUnsafeAsync()).ToList();
                var index = slots.FindIndex(s => s.Id == slot.Id);
                if (index >= 0)
                {
                    slots[index] = slot;
                }
                else
                {
                    slots.Add(slot);
                }
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(slots, _options));
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _gate.WaitAsync();
            try
            {
                var slots = (await ReadUnsafeAsync()).Where(slot => slot.Id != id).ToList();
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(slots, _options));
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<IReadOnlyCollection<DayOfWeek>> GetActiveDaysAsync()
        {
            var all = await GetAllAsync();
            return all.Select(slot => slot.DayOfWeek).Distinct().OrderBy(day => day).ToList();
        }

        private async Task<IReadOnlyList<TimetableSlot>> ReadUnsafeAsync()
        {
            if (!File.Exists(_filePath))
            {
                return Array.Empty<TimetableSlot>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<TimetableSlot>>(json, _options) ?? new List<TimetableSlot>();
        }
    }

    public class TimetableSlot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public int? CourseId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(8);
        public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(9);
        public string Room { get; set; } = string.Empty;
        public string Lecturer { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
