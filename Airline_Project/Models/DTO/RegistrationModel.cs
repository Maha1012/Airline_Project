using System.ComponentModel.DataAnnotations;

namespace UniversityWebApplnUsingAsp.Net.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string? Name { get; set; }
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Role { get; set; }

    }
}
