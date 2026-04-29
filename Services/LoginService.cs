using Delwings.Models.Results;
using System.Security.Claims;

namespace Delwings.Services
{
    public class LoginService
    {
        private readonly AccountService _accountService;
        
        public LoginService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<LoginResult?> LoginPrincipal(string login, string password)
        {
            var user = await _accountService.GetAccountByLoginAsync(login);
            if (user == null) { return null; }
            
            bool validity = await _accountService.CheckValidity(user, password);
            if (validity == false) { return null; }

            string userRole = user.Role.ToString();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login!),
                new Claim(ClaimTypes.Role, userRole)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            LoginResult result = new LoginResult
            {
                Principal = principal,
                UserRoleString = userRole
            };

            return result;
        }
    }
}
