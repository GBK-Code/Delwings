using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Delwings.Services.Dashboards;

namespace Delwings.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Cookies")]
    public class AdminController : Controller
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;
        private readonly PlaceService _placesService;
        private readonly OperatorPlacesService _operatorPlacesService;
        private readonly AdminDashboardService _adminDashboardService;

        public AdminController(
            AccountService accountService, 
            CourierApplicationService courierApplicationService, 
            PlaceService placeService, 
            OperatorPlacesService operatorPlacesService,
            AdminDashboardService adminDashboardService)
        { 
            _accountService = accountService;
            _courierApplicationService = courierApplicationService;
            _placesService = placeService;
            _operatorPlacesService = operatorPlacesService;
            _adminDashboardService = adminDashboardService;
        }

        private RedirectToActionResult Reload(string tab)
        {
            return RedirectToAction("Dashboard", "Admin", new { tab });
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName)) { return RedirectToAction("AcessDenied", "Home"); }

            var vm = _adminDashboardService.Build(tab, userName);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptCourier(int applicationId, int courierId)
        {
            var courier = await _accountService.GetAccountByIdAsync(courierId);
            if (courier == null) { return BadRequest(); }
            
            await _courierApplicationService.UpdateApplicationAsync(applicationId, 
                new CourierApplication {
                    Id = applicationId,
                    CourierId = courierId,
                    Name = courier.Name,
                    Surname = courier.Surname,
                    Phone = courier.Phone,
                    Status = CourierStatuses.Accepted
                }
            );

            return Reload("couriers");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineCourier(int applicationId, int courierId)
        {
            var courier = await _accountService.GetAccountByIdAsync(courierId);
            if (courier == null) { return BadRequest(); }

            await _courierApplicationService.UpdateApplicationAsync(applicationId,
                new CourierApplication
                {
                    Id = applicationId,
                    CourierId = courierId,
                    Name = courier.Name,
                    Surname = courier.Surname,
                    Phone = courier.Phone,
                    Status = CourierStatuses.Declined
                }
            );

            return Reload("couriers");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOperatorAsync(
                string name,
                string surname,
                string login,
                string phone,
                string email,
                string password,
                string passwordConfirm,
                int placeId
        )
        {
            if (password != passwordConfirm) { return RedirectToAction("BadReq", "Home"); }
            if (placeId == 0) { return RedirectToAction("BadReq", "Home"); }

            Account newOperator = new Account
            {
                Login = login,
                Password = password,
                Name = name,
                Surname = surname,
                Phone = phone,
                Email = email,
                Role = AccountRoles.Operator
            };

            int operatorId = await _accountService.CreateAccountAsync(newOperator);
            await _operatorPlacesService.CreateOperatorPlace(operatorId, placeId);

            return Reload("operators");
        }

        public async Task<IActionResult> DeleteOperator(int id)
        {
            var operatorPlaces = await _operatorPlacesService.GetAllOperatorPlacesAsync();
            var placeToDelete = operatorPlaces.Where(place => place.OperatorId == id).First();

            await _accountService.DeleteAccountByIdAsync(id);
            await _operatorPlacesService.DeleteOperatorPlace(placeToDelete.Id);

            return Reload("operators");
        }
        public async Task<IActionResult> AssignEmptySpace(int operatorId, int placeId)
        {
            var operPlace = await _operatorPlacesService.GetOperatorPlaceByOperatorIdAsync(operatorId);
            if (operPlace == null) { return RedirectToAction("BadReq", "Home"); }

            var place = await _placesService.GetPlaceByIdAsync(placeId);
            if (place == null) { return RedirectToAction("BadReq", "Home"); }

            operPlace.PlaceId = placeId;

            bool success = await _operatorPlacesService.UpdateOperatorPlace(operPlace.Id, operPlace);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return Reload("operators");
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
