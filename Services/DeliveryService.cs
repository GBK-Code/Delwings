using Delwings.Models;
using Delwings.Models.Basic;
using Microsoft.AspNetCore.Mvc;


namespace Delwings.Services
{
    public class DeliveryService
    {
        private readonly OrdersService _ordersService;
        private readonly CourierOrdersService _courierOrdersService;
        private readonly OrdersHistoryService _ordersHistoryService;

        public DeliveryService (OrdersService ordersService, CourierOrdersService courierOrdersService, OrdersHistoryService ordersHistoryService)
        {
            _ordersService = ordersService;
            _courierOrdersService = courierOrdersService;
            _ordersHistoryService = ordersHistoryService;
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

        public async Task<bool> OperatorConfirm(int orderId, int placeId, string trackId)
        {
            Order? orderToUpdate = await _ordersService.GetOrderByIdAsync(orderId);
            if (orderToUpdate == null) { return false; }

            orderToUpdate.CurrentLocationId = placeId;
            orderToUpdate.IsCarried = false;

            await _ordersService.UpdateOrderAsync(orderId, orderToUpdate);

            await _ordersHistoryService.CreateRecordAsync(new OrdersHistory
            {
                OrderId = orderId,
                PlaceId = placeId
            });

            var courierOrder = await _courierOrdersService.GetCourierOrderByToPlaceIdAsync(placeId);
            if (courierOrder != null)
            {
                await _courierOrdersService.DeleteCourierOrderAsync(courierOrder.Id);
            }

            return true;
        }

        public async Task<bool> CourierConfirm(int orderId, int courierId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return false; }

            order.IsCarried = true;

            await _ordersService.UpdateOrderAsync(orderId, order);

            var courierOrder = await _courierOrdersService.GetCourierOrderByCourierIdAsync(courierId);
            if (courierOrder == null) { return false; }

            return true;
        }
    }
}
