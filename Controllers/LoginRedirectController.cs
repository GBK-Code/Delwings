using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Delwings.Controllers
{
    public class LoginRedirectController : Controller
    {
        public IActionResult Dashboard()
        {
            string? role = User.FindFirstValue(ClaimTypes.Role);
            if (role == null) { return RedirectToAction("AccessDenied", "Home"); }

            return RedirectToAction("Dashboard", role, new { tab = "overview" } );
        }
    }
}
