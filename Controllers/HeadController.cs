using Delwings.Models;
using Delwings.Models.DTO;
using Delwings.Models.Enums;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Authentication;
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

        private RedirectToActionResult Reload(string tab)
        {
            return RedirectToAction("Dashboard", "Head", new { tab });
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            string? identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            var pageVM = await _headDashboardService.BuildAsync(tab, identityName);

            return View(pageVM);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(
                string name,
                string surname,
                string login,
                string phone,
                string email,
                string password,
                string passwordConfirm
        )
        {
            if (password != passwordConfirm) { return RedirectToAction("BadReq", "Home"); }

            CreateAccountDTO dto = new CreateAccountDTO()
            { 
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Phone = phone,
                Email = email
            };

            int accountId = await _accountRegistrationService.RegisterAdminAsync(dto);

            return Reload("admins");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var user = await _accountService.GetAccountByIdAsync(id);
            if (user == null || user.Role != AccountRoles.Admin) { return BadRequest(); }

            await _accountService.DeleteAccountByIdAsync(id);
            return Reload("admins");
        }

        [HttpPost]
        public async Task<IActionResult> AddPlace(
                string country,
                string city,
                string address,
                string contact,
                PlaceTypes placeType
        )
        {
            Place newPlace = new Place
            {
                Address = address,
                City = city,
                Country = country,
                Contact = contact,
                Type = placeType
            };

            await _placeService.CreatePlaceAsync(newPlace);

            return Reload("places");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var placeToDelete = await _placeService.GetPlaceByIdAsync(id);
            if (placeToDelete == null) { return BadRequest(); }

            await _placeService.DeletePlaceByIdAsync(id);
            return Reload("places");
        }


        [HttpGet]
        public async Task<IActionResult> ChooseTab(string tab)
        {
            return Reload(tab);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
