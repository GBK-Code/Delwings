using Delwings.Models.ViewModels;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Controllers
{
    [Authorize(Roles = "User", AuthenticationSchemes = "Cookies")]
    public class UserController : Controller
    {
        private readonly UserDashboardService _userDashboardService;

        public UserController(UserDashboardService userDashboardService)
        { 
            _userDashboardService = userDashboardService;
        }
        public async Task<IActionResult> Dashboard()
        {
            var identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            UserProfileVM? viewModel = await _userDashboardService.Build(identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            return View(viewModel);
        }
    }
}