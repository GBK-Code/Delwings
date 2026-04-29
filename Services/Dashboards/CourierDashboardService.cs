using Delwings.Models;

namespace Delwings.Services.Dashboards
{
    public class CourierDashboardService
    {
        private readonly AccountService _accountService;
        private readonly CourierApplicationService _courierApplicationService;
        private readonly CourierOrdersService _courierOrdersService;
        private readonly PlaceService _placeService;

        public CourierDashboardService(
            AccountService accountService,
            CourierApplicationService courierApplicationService,
            CourierOrdersService courierOrdersService,
            PlaceService placeService)
        {
            _accountService = accountService;
            _courierApplicationService = courierApplicationService;
            _courierOrdersService = courierOrdersService;
            _placeService = placeService;
        }
        public async Task<CourierProfileVM?> Build(string tab, string identityName)
        {
            var user = await _accountService.GetAccountByLoginAsync(identityName);
            if (user == null) { return null; }

            var status = await _courierApplicationService.GetStatusByCourierIdAsync(user.Id);

            var courierOrders = await _courierOrdersService.GetAllCourierOrdersAsync();
            var myCourierOrders = courierOrders.Where(order => order.CourierId == user.Id);

            List<Place> fromPlaces = new List<Place>();
            List<Place> toPlaces = new List<Place>();

            foreach (var courierOrder in courierOrders)
            {
                var fromPlace = await _placeService.GetPlaceByIdAsync(courierOrder.FromPlaceId);
                var toPlace = await _placeService.GetPlaceByIdAsync(courierOrder.ToPlaceId);
                if (fromPlace == null || toPlace == null) { return null; }

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

            return viewModel;
        }
    }
}
