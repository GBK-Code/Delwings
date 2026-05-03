using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Models.ViewModels;
using Delwings.Models.ViewModels.Tables;


namespace Delwings.Services.Dashboards
{
    public class OperatorVMService
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;
        private readonly OperatorPlacesService _operatorPlacesService;
        private readonly PlaceService _placeService;
        private readonly CourierOrdersService _courierOrdersService;
        private readonly CourierApplicationService _courierApplicationService;

        public OperatorVMService
            (
                AccountService accountService,
                OrdersService ordersService,
                OperatorPlacesService operatorPlacesService,
                PlaceService placeService,
                CourierOrdersService courierOrdersService,
                CourierApplicationService courierApplicationService
            )
        {
            _accountService = accountService;
            _ordersService = ordersService;
            _operatorPlacesService = operatorPlacesService;
            _placeService = placeService;
            _courierOrdersService = courierOrdersService;
            _courierApplicationService = courierApplicationService;
        }

        public async Task<OperatorPageVM?> BuildDashboard(string tab, string identityName)
        {
            var courierList = await _accountService.GetAllAccountsAsync();
            courierList = courierList.Where(user => user.Role == AccountRoles.Courier).ToList();

            List<Account> acceptedCouriers = new List<Account>();

            foreach (var account in courierList)
            {
                CourierStatuses courierStatus = await _courierApplicationService.GetStatusByCourierIdAsync(account.Id);
                if (courierStatus == CourierStatuses.Accepted)
                {
                    acceptedCouriers.Add(account);
                }
            }

            var oper = await _accountService.GetAccountByLoginAsync(identityName);
            if (oper == null) { return null; }

            var orders = await _ordersService.GetAllOrdersAsync();
            var places = await _placeService.GetAllPlacesAsync();
            var courierOrders = await _courierOrdersService.GetAllCourierOrdersAsync();

            Place? workingPlace = await GetOperatorWorkingPlaceByIdentityAsync(identityName);
            if (workingPlace == null) { return null; }

            List<OrderRowVM> orderRows = await GetOrdersTableRows(orders);

            var viewModel = new OperatorPageVM
            {
                Me = oper,
                Tab = tab,
                OrdersList = orders.ToList(),
                OrderRows = orderRows,
                WorkingPlace = workingPlace,
                PlacesList = places.ToList(),
                AcceptedCourierList = acceptedCouriers,
                CourierOrders = courierOrders
            };

            return viewModel;
        }

        public async Task<EditOrderPAgeVM?> BuildEditOrderPage(int id)
        {
            Order? orderData = await _ordersService.GetOrderByIdAsync(id);
            if (orderData == null) { return null; }
            Account? courier = null;

            if (orderData.CourierId != null)
            {
                courier = await _accountService.GetAccountByIdAsync(orderData.CourierId.Value);
                if (courier == null) { return null; }
            }

            var vm = new EditOrderPAgeVM
            {
                Order = orderData,
                Courier = courier
            };

            return vm;
        }

        public async Task<List<OrderRowVM>> GetOrdersTableRows(List<Order> orders)
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            var places = await _placeService.GetAllPlacesAsync();

            List<OrderRowVM> rows = new List<OrderRowVM>();

            foreach (Order order in orders)
            {
                Place? place = places.Where(plc => plc.Id == order.CurrentLocationId).FirstOrDefault();
                Account? courier = accounts.Where(acc => acc.Id == order.CourierId).FirstOrDefault();

                var row = new OrderRowVM
                {
                    OrderRow = order,
                    PlaceRow = place,
                    AccountRow = courier
                };

                rows.Add(row);
            }

            return rows;
        }

        public async Task<Place?> GetOperatorWorkingPlaceByIdentityAsync(string identityName)
        {
            var oper = await _accountService.GetAccountByLoginAsync(identityName);
            if (oper == null) { return null; }

            var operatorPlace = await _operatorPlacesService.GetOperatorPlaceByOperatorIdAsync(oper.Id);
            if (operatorPlace == null) { return null; }

            var workingPlace = await _placeService.GetPlaceByIdAsync(operatorPlace.PlaceId);
            if (workingPlace == null) { return null; }

            return workingPlace;
        }
    }
}
