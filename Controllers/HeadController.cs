using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace Delwings.Controllers
{
    [Authorize(Roles = "Head", AuthenticationSchemes = "Cookies")]
    public class HeadController : Controller
    {
        private readonly AccountService _accountService;
        private readonly PlaceService _placeService;

        public HeadController(AccountService accountService, PlaceService placeService)
        {
            _accountService = accountService;
            _placeService = placeService;
        }

        private RedirectToActionResult Reload(string tab)
        {
            return RedirectToAction("Dashboard", "Head", new { tab });
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            var identityName = User.Identity?.Name;
            if (identityName.IsNullOrEmpty()) { return RedirectToAction("AccessDenied", "Home"); }

            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return RedirectToAction("AccessDenied", "Home"); }

            var users = await _accountService.GetAllAccountsAsync();
            List<Account> _admins = users.Where(user => user.Role == AccountRoles.Admin).ToList();

            var places = await _placeService.GetAllPlacesAsync();

            ViewBag.PlaceTypes = Enum.GetValues(typeof(PlaceTypes))
                .Cast<PlaceTypes>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            var pageVM = new HeadPageVM
            {
                Me = user,
                Tab = tab,
                AdminAccounts = _admins,
                Places = places
            };

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
            if (password != passwordConfirm)
            {
                return RedirectToAction("BadReq", "Home");
            }

            Account newAccount = new Account
            {
                Name = name,
                Surname = surname,
                Login = login,
                Phone = phone,
                Email = email,
                Password = password,
                Role = AccountRoles.Admin
            };

            await _accountService.CreateAccountAsync(newAccount);

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
