using System.Security.Claims;

namespace Delwings.Models.Results
{
    public class LoginResult
    {
        public ClaimsPrincipal? Principal { get; set; }
        public string? UserRoleString { get; set; }
    }
}
