using UniversityWebApplnUsingAsp.Net.Models.Domain;

namespace UniversityWebApplnUsingAsp.Net.Models.DTO
{
    public class LoginResponse : Status
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string UserId { get; internal set; }
        
        public List<string> Role { get; set; } // Change the data type to List<string>


    }
}
