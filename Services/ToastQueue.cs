using System.Collections.Concurrent;
using Microsoft.JSInterop;

namespace FinalAssignemnt_APDP.Services;

public enum ToastType { Success, Error, Warning, Info }

public sealed class ToastQueue
{
    private readonly ConcurrentQueue<(ToastType Type, string Message, string Title)> _queue = new();
    public void Enqueue(string message, ToastType type = ToastType.Info, string? title = null)
        => _queue.Enqueue((type, message, title ?? GetDefaultTitle(type)));

    public async Task FlushAsync(IJSRuntime jsRuntime)
    {
        while (_queue.TryDequeue(out var item))
        {
            var fn = item.Type switch
            {
                ToastType.Success => "showSuccess",
                ToastType.Error => "showError",
                ToastType.Warning => "showWarning",
                _ => "showInfo"
            };
            await jsRuntime.InvokeVoidAsync(fn, item.Message, item.Title);
        }
    }

    private static string GetDefaultTitle(ToastType type) => type switch
    {
        ToastType.Success => "Thành công",
        ToastType.Error => "L?i",
        ToastType.Warning => "C?nh báo",
        _ => "Thông báo"
    };
}
