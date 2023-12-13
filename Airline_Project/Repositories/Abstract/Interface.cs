using System.Security.Claims;
using UniversityWebApplnUsingAsp.Net.Models.DTO;

namespace UniversityWebApplnUsingAsp.Net.Respositories.Abstract
{
    public interface Interface
    {
        public interface ITokenService
        {
            TokenResponse GetToken(IEnumerable<Claim> claim);
            string GetRefreshToken();
            ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        }
    }
}
