using Delwings.Models.Requests;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delwings.Controllers
{
    [Authorize(Roles = "Head", AuthenticationSchemes = "Cookies")]
    public class HeadController : Controller
    {
        private readonly AccountService _accountService;
        private readonly PlaceService _placeService;
        private readonly HeadDashboardService _headDashboardService;
        private readonly AccountRegistrationService _accountRegistrationService;

        public HeadController 
        (
            AccountService accountService, 
            PlaceService placeService, 
            HeadDashboardService headDashboardService, 
            AccountRegistrationService accountRegistrationService
        )
        {
            _accountService = accountService;
            _placeService = placeService;
            _headDashboardService = headDashboardService;
            _accountRegistrationService = accountRegistrationService;
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            string identityName = User.Identity!.Name!;
            var pageVM = await _headDashboardService.BuildAsync(tab, identityName);

            return View(pageVM);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(CreateAccountRequest request)
        {
            if (request.Password != request.PasswordConfirm) { return RedirectToAction("BadReq", "Home"); }
            int accountId = await _accountRegistrationService.RegisterAdminAsync(request);

            return RedirectToAction("Dashboard", "Head", new { tab = "admins" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var success = await _accountService.DeleteAdminAccountByIdAsync(id);
            if (!success) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Head", new { tab = "admins" });
        }

        [HttpPost]
        public async Task<IActionResult> AddPlace(CreatePlaceRequest request)
        {
            await _placeService.CreatePlaceAsync(request);
            return RedirectToAction("Dashboard", "Head", new { tab = "places" });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlace(int id)
        {           
            var success = await _placeService.DeletePlaceByIdAsync(id);
            if (success == false) { return BadRequest(); }

            return RedirectToAction("Dashboard", "Head", new { tab = "places" });
        }
    }
}
