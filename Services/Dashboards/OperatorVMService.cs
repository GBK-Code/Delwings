using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Models.ViewModels;


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

            var operatorPlace = await _operatorPlacesService.GetOperatorPlaceByOperatorIdAsync(oper.Id);
            if (operatorPlace == null) { return null; }

            var places = await _placeService.GetAllPlacesAsync();

            var workingPlace = await _placeService.GetPlaceByIdAsync(operatorPlace.PlaceId);
            if (workingPlace == null) { return null; }

            var courierOrders = await _courierOrdersService.GetAllCourierOrdersAsync();

            var viewModel = new OperatorPageVM
            {
                Me = oper,
                Tab = tab,
                OrdersList = orders.ToList(),
                WorkingPlace = workingPlace,
                PlacesList = places.ToList(),
                AcceptedCourierList = acceptedCouriers,
                CourierOrders = courierOrders
            };

            return viewModel;
        }

        public async Task<EditOrderPAgeVM?> BuildEditOrderPage(OrderRequest request)
        {
            OrderTypes type = request.OrderType;

            Order orderData = _ordersService.BuildOrder(request);
            Account? courier = null;

            if (request.CourierId != null)
            {
                courier = await _accountService.GetAccountByIdAsync(request.CourierId.Value);
                if (courier == null) { return null; }
            }

            var vm = new EditOrderPAgeVM
            {
                Order = orderData,
                Courier = courier
            };

            return vm;
        }
    }
}
