using Delwings.Models.Basic;
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
        private readonly TableFilterService _tableFilterService;

        public HeadController 
        (
            AccountService accountService, 
            PlaceService placeService, 
            HeadDashboardService headDashboardService, 
            AccountRegistrationService accountRegistrationService,
            TableFilterService tableFilterService
        )
        {
            _accountService = accountService;
            _placeService = placeService;
            _headDashboardService = headDashboardService;
            _accountRegistrationService = accountRegistrationService;
            _tableFilterService = tableFilterService;
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

        [HttpGet]
        public async Task<IActionResult> FilterPlaces(PlacesFilterRequest request)
        {
            string identityName = User.Identity!.Name!;

            List<Place> filtered = await _tableFilterService.GetFilteredPlaces(request);

            var viewModel = await _headDashboardService.BuildAsync("tables", identityName);

            if (viewModel == null) { return RedirectToAction("BadReq", "Home"); }
            viewModel.Places = filtered;

            return PartialView("_PlaceListCard", viewModel);
        }
    }
}
