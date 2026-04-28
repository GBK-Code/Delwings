using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Services;
using Delwings.Services.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Delwings.Controllers
{
    [Authorize(Roles = "Operator", AuthenticationSchemes = "Cookies")]
    public class OperatorController: Controller
    {
        private readonly OrdersService _ordersService;
        private readonly CourierOrdersService _courierOrdersService;
        private readonly OperatorVMService _operatorVMService;
        
        public OperatorController 
            (
                OrdersService ordersService,
                CourierOrdersService courierOrdersService,
                OperatorVMService operatorVMService
            )
        {
            _ordersService = ordersService;
            _courierOrdersService = courierOrdersService;
            _operatorVMService = operatorVMService;
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            var identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            var viewModel = _operatorVMService.BuildDashboard(tab, identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            return View(viewModel);
        }
        public async Task<IActionResult> OrderFound(int orderId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            return View(order);
        }
        public async Task<IActionResult> EditOrder(OrderRequest request)
        {
            EditOrderPAgeVM? viewModel = await _operatorVMService.BuildEditOrderPage(request);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }
            
            return View(viewModel);
        }

        public async Task<IActionResult> SubmitEdit(OrderRequest request)
        {
            var orderData = await _ordersService.BuildOrder(request);
            await _ordersService.UpdateOrderAsync(orderData.Id, orderData);

            return RedirectToAction("Dashboard", new { tab = "orders"} );
        }

        public async Task<IActionResult> AddOrder(OrderRequest request)
        {
            await _ordersService.CreateOrderAsync(request);
            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            await _ordersService.DeleteOrderAsync(orderId);
            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        public async Task<IActionResult> ReleaseOrder(int receiverNumber)
        {
            var order = await _ordersService.GetOrderByReceiverNumberAsync(receiverNumber);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("OrderFound", "Operator", new { orderId = order.Id});
        }

        public async Task<IActionResult> AssignOrderPlace(string trackId, int placeId)
        {
            var order = await _ordersService.GetOrderByTrackIdAsync(trackId);
            if (order == null) { RedirectToAction("Dashboard", "Operator"); }

            order.CurrentLocationId = placeId;
            await _ordersService.UpdateOrderAsync(order.Id, order);

            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        public async Task<IActionResult> RedirectCourier(int orderId, int courierId, int placeId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            await _courierOrdersService.CreateCourierOrderAsync(new CourierOrder
            {
                CourierId = courierId,
                FromPlaceId = order.CurrentLocationId,
                ToPlaceId = placeId,
                OrderId = orderId
            });
                
            await _ordersService.UpdateOrderAsync(order.Id, order);

            return RedirectToAction("Dashboard", new { tab = "redirection" });
        }
    }
}
