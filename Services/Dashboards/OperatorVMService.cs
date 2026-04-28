using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Models.Requests;


namespace Delwings.Services.Dashboards
{
    public class OperatorVMService
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;
        private readonly OperatorPlacesService _operatorPlacesService;
        private readonly PlaceService _placeService;
        private readonly CourierOrdersService _courierOrdersService;

        public OperatorVMService 
            (
                AccountService accountService, 
                OrdersService ordersService, 
                OperatorPlacesService operatorPlacesService, 
                PlaceService placeService, 
                CourierOrdersService courierOrdersService
            )
        {
            _accountService = accountService;
            _ordersService = ordersService;
            _operatorPlacesService = operatorPlacesService;
            _placeService = placeService;
            _courierOrdersService = courierOrdersService;
        }

        public async Task<OperatorPageVM?> BuildDashboard(string tab, string identityName)
        {
            var courierList = await _accountService.GetAllAccountsAsync();
            courierList = courierList.Where(user => user.Role == AccountRoles.Courier).ToList();

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
                CourierList = courierList,
                CourierOrders = courierOrders
            };

            return viewModel;
        }

        public async Task<EditOrderPAgeVM?> BuildEditOrderPage(OrderRequest request)
        {
            OrderTypes type = request.OrderType;

            Order orderData = await _ordersService.BuildOrder(request);
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
