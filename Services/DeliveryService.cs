using Delwings.Models;


namespace Delwings.Services
{
    public class DeliveryService
    {
        private readonly OrdersService _ordersService;
        private readonly CourierOrdersService _courierOrdersService;

        public DeliveryService (OrdersService ordersService, CourierOrdersService courierOrdersService)
        {
            _ordersService = ordersService;
            _courierOrdersService = courierOrdersService;
        }

        public async Task<bool> RedirectCourier(int orderId, int courierId, int placeId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return false; }

            await _courierOrdersService.CreateCourierOrderAsync(new CourierOrder
            {
                CourierId = courierId,
                FromPlaceId = order.CurrentLocationId,
                ToPlaceId = placeId,
                OrderId = orderId
            });

            return true;
        }
    }
}
