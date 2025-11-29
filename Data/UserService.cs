using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalAssignemnt_APDP.Data
{
    public class UserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IDbContextFactory<ApplicationDbContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContextFactory = dbContextFactory;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get all users with their assigned roles
        /// </summary>
        public async Task<List<(ApplicationUser User, List<string> Roles)>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<(ApplicationUser, List<string>)>();

            foreach (var user in users)
            {
                var roles = (await _userManager.GetRolesAsync(user)).ToList();
                result.Add((user, roles));
            }

            return result;
        }

        /// <summary>
        /// Get a user by their ID
        /// </summary>
        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Get all roles assigned to a user
        /// </summary>
        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        /// <summary>
        /// Create a new user with the specified password and roles
        /// </summary>
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, List<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded && roles.Any())
            {
                await _userManager.AddToRolesAsync(user, roles);
            }
            return result;
        }

        /// <summary>
        /// Update user information
        /// </summary>
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Update the roles assigned to a user
        /// </summary>
        public async Task<IdentityResult> UpdateUserRolesAsync(ApplicationUser user, List<string> newRoles)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(newRoles).ToList();
            var rolesToAdd = newRoles.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Get all available roles in the system
        /// </summary>
        public async Task<List<string>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }
    }
}
