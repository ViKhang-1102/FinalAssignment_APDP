using Microsoft.AspNetCore.Identity;

namespace FinalAssignemnt_APDP.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public DateTime? DOB { get; set; }
        public string? Avatar { get; set; }
        public string? StudentId { get; set; }
    }

}
