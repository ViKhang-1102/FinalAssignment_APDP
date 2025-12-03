using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalAssignemnt_APDP.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinalAssignemnt_APDP.Components.Pages.LecturerPages
{
    public abstract class LecturerTeachingPageBase : ComponentBase
    {
        protected bool IsLoading { get; private set; } = true;
        protected string? ErrorMessage { get; private set; }
        protected LecturerWorkspaceResult Workspace { get; private set; } = LecturerWorkspaceResult.Empty;
        protected string DisplayName { get; private set; } = "Lecturer";

        [Inject]
        protected LecturerWorkspaceService WorkspaceService { get; set; } = default!;

        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var authState = await AuthenticationStateTask;
                var user = authState.User;

                if (user?.Identity?.IsAuthenticated != true)
                {
                    ErrorMessage = "You must be signed in to view this page.";
                    return;
                }

                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    ErrorMessage = "Unable to resolve your account identifier.";
                    return;
                }

                DisplayName = string.IsNullOrWhiteSpace(user.Identity?.Name) ? "Lecturer" : user.Identity!.Name!;
                Workspace = await WorkspaceService.LoadAsync(userId);
                await AfterWorkspaceLoadedAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unable to load lecturer data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected virtual Task AfterWorkspaceLoadedAsync() => Task.CompletedTask;
    }
}
