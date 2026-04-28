using Delwings.Models;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Controllers
{
    [Authorize(Roles = "Courier", AuthenticationSchemes = "Cookies")]
    public class CourierController : Controller
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;
        private readonly PlaceService _placeService;
        private readonly CourierOrdersService _courierOrdersService;

        public CourierController (
            AccountService service,
            CourierApplicationService courierApplicationService,
            OrdersService ordersService,
            PlaceService placeService,
            CourierOrdersService courierOrdersService)
        { 
            _accountService = service;
            _courierApplicationService = courierApplicationService;
            _placeService = placeService;
            _courierOrdersService = courierOrdersService;
        }

        private RedirectToActionResult Reload(string tab)
        {
            return RedirectToAction("Dashboard", "Courier", new { tab });
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            var identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return RedirectToAction("AccessDenied", "Home"); }

            var status = await _courierApplicationService.GetStatusByCourierIdAsync(user.Id);

            var courierOrders = await _courierOrdersService.GetAllCourierOrdersAsync();
            var myCourierOrders = courierOrders.Where(order => order.CourierId == user.Id);

            List<Place> fromPlaces = new List<Place>();
            List<Place> toPlaces = new List<Place>();

            foreach (var courierOrder in courierOrders)
            {
                var fromPlace = await _placeService.GetPlaceByIdAsync(courierOrder.FromPlaceId);
                var toPlace = await _placeService.GetPlaceByIdAsync(courierOrder.ToPlaceId);
                if (fromPlace == null || toPlace == null) { return RedirectToAction("BadReq", "Home"); }

                fromPlaces.Add(fromPlace);
                toPlaces.Add(toPlace);
            }

            var viewModel = new CourierProfileVM
            {
                Account = user,
                Tab = tab,
                Status = status,
                FromPlacesList = fromPlaces,
                ToPlacesList = toPlaces,
                CourierOrders = courierOrders
            };

            return View(viewModel);
        }


        public async Task<IActionResult> ChooseTab(string tab)
        {
            return Reload(tab);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
