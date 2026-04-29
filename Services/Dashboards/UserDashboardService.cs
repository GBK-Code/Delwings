using Delwings.Models.Basic;
using Delwings.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Delwings.Services.Dashboards
{
    public class UserDashboardService
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;

        public UserDashboardService(AccountService accountService, OrdersService ordersService)
        {
            _accountService = accountService;
            _ordersService = ordersService;
        }

        public async Task<UserProfileVM?> Build(string identityName)
        {
            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return null; }

            var orders = await _ordersService.GetAllOrdersAsync();
            var myOrders = orders.Where(order => order.SenderId == user.Id).ToList();

            List<Account> couriersList = [.. new Account[myOrders.Count]];

            for (int i = 0; i < myOrders.Count; i++)
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

            return viewModel;
        }
    }
}
