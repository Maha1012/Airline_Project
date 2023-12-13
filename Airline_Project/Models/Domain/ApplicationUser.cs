using Microsoft.AspNetCore.Identity;

namespace UniversityWebApplnUsingAsp.Net.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
