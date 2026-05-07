using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.ViewModels;


namespace Delwings.Services.Dashboards
{
    public class HeadDashboardService
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;
        private readonly PlaceService _placeService;
        private readonly DeveloperPageService _developerPageService;

        public HeadDashboardService(AccountService accountService, PlaceService placeService, DeveloperPageService developerPageService, OrdersService ordersService)
        {
            _accountService = accountService;
            _placeService = placeService;
            _developerPageService = developerPageService;
            _ordersService = ordersService;
        }

        public async Task<HeadPageVM?> BuildAsync(string tab, string identityName)
        {
            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return null; }

            List<Order> orders = await _ordersService.GetAllOrdersAsync();
            int numberOfOrders = orders.Count;

            var users = await _accountService.GetAllAccountsAsync();
            List<Account> _admins = users.Where(user => user.Role == AccountRoles.Admin).ToList();

            var places = await _placeService.GetAllPlacesAsync();

            // double bubbleSortTime = await _developerPageService.GetBubbleSortTime(orders, 1);
            double LINQSortResult = await _developerPageService.GetLINQSortTime(orders, 1);

            DevToolsModel devTools = new DevToolsModel()
            {
                NumberOfElements = numberOfOrders,
                QuickSortTime = LINQSortResult
            };

            var pageVM = new HeadPageVM
            {
                Me = user,
                Tab = tab,
                Orders = orders,
                Accounts = users,
                AdminAccounts = _admins,
                Places = places,
                DeveloperModel = devTools
            };

            return pageVM;
        }
    }
}
