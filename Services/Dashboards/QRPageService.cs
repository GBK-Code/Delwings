using Delwings.Models.Basic;
using Delwings.Models.ViewModels;

namespace Delwings.Services.Dashboards
{
    public class QRPageService
    {
        private readonly ApiService _apiService;
        private readonly OrdersService _ordersService;

        public QRPageService(ApiService apiService, OrdersService ordersService)
        {
            _apiService = apiService;
            _ordersService = ordersService;
        }

        public async Task<QRPageVM?> Build(int orderId)
        {
            Order? order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return null; }
            if (order.TrackId == null) { return null; }

            string QRapi = _apiService.GetQRApi(order.TrackId);

            var vm = new QRPageVM()
            { 
                QrURL = QRapi,
                OrderData = order
            };

            return vm;
        }
    }
}
