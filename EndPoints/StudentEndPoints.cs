using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FinalAssignemnt_APDP.Data;
using Microsoft.EntityFrameworkCore;
namespace FinalAssignemnt_APDP.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this WebApplication app)
        {
            app.MapGet("/api/students/search", async (
                IDbContextFactory<ApplicationDbContext> dbFactory,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                string? q,
                int page = 1) =>
            {
                var studentRole = await roleManager.FindByNameAsync("Student");
                if (studentRole == null)
                {
                    return Results.Ok(new List<object>());
                }

                using var context = dbFactory.CreateDbContext();
                var studentUserIds = await context.UserRoles
                    .Where(ur => ur.RoleId == studentRole.Id)
                    .Select(ur => ur.UserId)
                    .ToListAsync();

                var query = userManager.Users.Where(u => studentUserIds.Contains(u.Id));

                if (!string.IsNullOrWhiteSpace(q))
                {
                    query = query.Where(u =>
                        u.Name.Contains(q) ||
                        u.Email.Contains(q) ||
                        (u.StudentId != null && u.StudentId.Contains(q)));
                }

                var students = await query
                    .Skip((page - 1) * 10)
                    .Take(10)
                    .Select(u => new
                    {
                        id = u.Id,
                        name = u.Name,
                        email = u.Email ?? "",
                        studentId = u.StudentId ?? ""
                    })
                    .ToListAsync();

                return Results.Ok(students);
            });
        }
    }
}
