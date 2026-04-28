using Microsoft.AspNetCore.Mvc;
using Delwings.Services;
using Delwings.Models;
using Microsoft.AspNetCore.Authorization;

namespace Delwings.Controllers
{
    public class TrackController: Controller
    {
        private readonly OrdersService _ordersService;
        private readonly PlaceService _placeService;
        private readonly AccountService _accountService;
        private readonly OperatorPlacesService _operatorPlaceService;
        private readonly OrdersHistoryService _ordersHistoryService;
        private readonly CourierOrdersService _courierOrdersService;

        public TrackController(
            OrdersService ordersService, 
            PlaceService placeService, 
            AccountService accountService,
            OperatorPlacesService operatorPlacesService,
            OrdersHistoryService ordersHistoryService,
            CourierOrdersService courierOrdersService)
        { 
            _ordersService = ordersService;
            _placeService = placeService;
            _accountService = accountService;
            _operatorPlaceService = operatorPlacesService;
            _ordersHistoryService = ordersHistoryService;
            _courierOrdersService = courierOrdersService;
        }

        public async Task<IActionResult> Index(string trackToken)
        {
            var order = await _ordersService.GetOrderByTrackIdAsync(trackToken);
            if (order == null) { return RedirectToAction("AccessDenied", "Home"); }

            var place = await _placeService.GetPlaceByIdAsync(order.CurrentLocationId);

            var history = await _ordersHistoryService.GetAllHistoryAsync();
            history = history.Where(rec => rec.OrderId == order.Id).ToList();

            List<Place> visited = new List<Place>();

            foreach (var record in history)
            {
                var placeToAdd = await _placeService.GetPlaceByIdAsync(record.PlaceId);
                if (placeToAdd != null) { visited.Add(placeToAdd); }
            }
            Account? operatorAccount = null;
            Place? placeToAssign = null;

            if (User.IsInRole("Operator"))
            {
                string? identityName = User.Identity?.Name;
                if (identityName == null) { return RedirectToAction("AccessDenied", "Home"); }

                var oper = await _accountService.GetAccountByLoginAsync(identityName);
                if (oper == null) { return RedirectToAction("AccessDenied", "Home"); }

                operatorAccount = oper;

                // Assign place
                var operatorPlace = await _operatorPlaceService.GetOperatorPlaceByOperatorIdAsync(operatorAccount.Id);
                if (operatorPlace == null) { return RedirectToAction("AccessDenied", "Home"); }

                var placeId = operatorPlace.PlaceId;
                placeToAssign = await _placeService.GetPlaceByIdAsync(placeId);
                if (placeToAssign == null) { return RedirectToAction("AccessDenied", "Home"); }
            }

            Account? courier = null;

            if (User.IsInRole("Courier"))
            {
                string? identityName = User.Identity?.Name;
                if (identityName == null) { return RedirectToAction("AccessDenied", "Home"); }

                courier = await _accountService.GetAccountByLoginAsync(identityName);
                if (courier == null) { return RedirectToAction("AccessDenied", "Home"); }
            }

            var trackVM = new TrackerPageVM
            {
                Order = order,
                Current = place,
                IsOperator = User.IsInRole("Operator"),
                IsCourier = User.IsInRole("Courier"),
                Operator = operatorAccount,
                Courier = courier,
                OperatorPlace = placeToAssign,
                OrderPlaces = visited
            };

            return View(trackVM);
        }

        [HttpPost]
        [Authorize(Roles = "Operator")]
        public async Task<IActionResult> AssignPlace(int orderId, int placeId, string trackId)
        {
            Order? orderToUpdate = await _ordersService.GetOrderByIdAsync(orderId);
            if (orderToUpdate == null) { return RedirectToAction("BadReq", "Home"); }

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

            return RedirectToAction("Index", "Track", new { trackToken = trackId });
        }

        [HttpPost]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> CourierConfirm(int orderId, int courierId, string trackId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            order.IsCarried = true;

            await _ordersService.UpdateOrderAsync(orderId, order);

            var courierOrder = await _courierOrdersService.GetCourierOrderByCourierIdAsync(courierId);
            if (courierOrder == null) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Index", "Track", new {trackToken = trackId});
        }
    }
}
