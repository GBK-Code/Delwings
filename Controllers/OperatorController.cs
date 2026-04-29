using Delwings.Models;
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
        private readonly OperatorVMService _operatorVMService;
        private readonly DeliveryService _deliveryService;
        
        public OperatorController 
            (
                OrdersService ordersService,
                OperatorVMService operatorVMService,
                DeliveryService deliveryService
            )
        {
            _ordersService = ordersService;
            _operatorVMService = operatorVMService;
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string tab)
        {
            string? identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            OperatorPageVM? viewModel = await _operatorVMService.BuildDashboard(tab, identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OrderFound(int orderId)
        {
            Order? order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(OrderRequest request)
        {
            EditOrderPAgeVM? viewModel = await _operatorVMService.BuildEditOrderPage(request);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEdit(OrderRequest request)
        {
            Order orderData = _ordersService.BuildOrder(request);
            await _ordersService.UpdateOrderAsync(orderData.Id, orderData);

            return RedirectToAction("Dashboard", new { tab = "orders"} );
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderRequest request)
        {
            await _ordersService.CreateOrderAsync(request);
            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            bool success = await _ordersService.DeleteOrderAsync(orderId);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        [HttpGet]
        public async Task<IActionResult> ReleaseOrder(int receiverNumber)
        {
            Order? order = await _ordersService.GetOrderByReceiverNumberAsync(receiverNumber);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("OrderFound", "Operator", new { orderId = order.Id});
        }

        [HttpPost]
        public async Task<IActionResult> AssignOrderPlace(string trackId, int placeId)
        {
            bool success = await _ordersService.ReassignOrderPlaceAsync(trackId, placeId);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Dashboard", new { tab = "orders" });
        }

        [HttpPost]
        public async Task<IActionResult> RedirectCourier(int orderId, int courierId, int placeId)
        {
            bool success = await _deliveryService.RedirectCourier(orderId, courierId, placeId);
            if (!success) { return RedirectToAction("BadReq", "Home"); }

            return RedirectToAction("Dashboard", new { tab = "redirection" });
        }
    }
}
