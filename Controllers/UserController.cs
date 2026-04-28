using Delwings.Models;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Controllers
{
    [Authorize(Roles = "User", AuthenticationSchemes = "Cookies")]
    public class UserController : Controller
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;

        public UserController(AccountService service, OrdersService ordersService) { 
            _accountService = service;
            _ordersService = ordersService;
        }
        public async Task<IActionResult> Dashboard()
        {
            var identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return RedirectToAction("AccessDenied", "Home"); }

            var orders = await _ordersService.GetAllOrdersAsync();
            var myOrders = orders.Where(order => order.SenderId == user.Id).ToList();

            List<Account> couriersList = new List<Account>(new Account[myOrders.Count]);

            for (int i=0;i <  myOrders.Count;i++)
            {
                var mOrder = myOrders[i];
                if (mOrder.CourierId == null) continue;

                var courier = await _accountService.GetAccountByIdAsync(mOrder.CourierId.Value);
                if (courier == null) continue;

                couriersList[i] = courier;
            }

            var viewModel = new UserProfileVM
            {
                Me = user,
                MyOrdersList = myOrders,
                CourierList = couriersList
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}