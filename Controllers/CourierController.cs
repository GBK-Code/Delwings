using Delwings.Models;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delwings.Controllers
{
    [Authorize(Roles = "Courier", AuthenticationSchemes = "Cookies")]
    public class CourierController : Controller
    {
        private readonly CourierDashboardService _courierDashboardService;

        public CourierController (CourierDashboardService courierDashboardService)
        { 
            _courierDashboardService = courierDashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string tab)
        {
            string? identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            CourierProfileVM? viewModel = await _courierDashboardService.Build(tab, identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            return View(viewModel);
        }

    }
}
