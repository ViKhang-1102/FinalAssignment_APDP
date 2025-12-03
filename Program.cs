using FinalAssignemnt_APDP.Components;
using FinalAssignemnt_APDP.Components.Account;
using FinalAssignemnt_APDP.Data;
using FinalAssignemnt_APDP.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ToastQueue>();
builder.Services.AddScoped<LecturerWorkspaceService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await UserSeeder.SeedRolesAndAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapGet("/api/grades/export", async (IDbContextFactory<ApplicationDbContext> dbFactory) =>
{
    await using var context = await dbFactory.CreateDbContextAsync();
    var grades = await context.Grades
        .Include(g => g.Course)
        .Include(g => g.Student)
        .OrderBy(g => g.StudentID)
        .ThenBy(g => g.CourseID)
        .ToListAsync();

    var sb = new StringBuilder();
    sb.AppendLine("StudentId,CourseId,Midterm,Final,Average,Letter,Note");
    foreach (var grade in grades)
    {
        sb.AppendLine(string.Join(',',
            FormatCsv(grade.StudentID),
            FormatCsv(grade.CourseID),
            FormatCsv(grade.MidtermScore),
            FormatCsv(grade.FinalScore),
            FormatCsv(grade.AverageScore),
            FormatCsv(grade.LetterGrade),
            FormatCsv(grade.Note)));
    }

    static string FormatCsv(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var formatted = value switch
        {
            double d => d.ToString("0.##", CultureInfo.InvariantCulture),
            float f => f.ToString("0.##", CultureInfo.InvariantCulture),
            decimal m => m.ToString("0.##", CultureInfo.InvariantCulture),
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };

        if (formatted.IndexOfAny(new[] { ',', '\n', '"' }) >= 0)
        {
            formatted = "\"" + formatted.Replace("\"", "\"\"") + "\"";
        }

        return formatted;
    }

    return Results.File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"grades_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
});

app.Run();
