using Delwings.Models.Basic;
using Delwings.Models.Requests;
using Delwings.Models.ViewModels;
using Delwings.Models.ViewModels.Tables;
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
        private readonly TableFilterService _tableFilterService;
        
        public OperatorController 
            (
                OrdersService ordersService,
                OperatorVMService operatorVMService,
                DeliveryService deliveryService,
                TableFilterService tableFilterService
            )
        {
            _ordersService = ordersService;
            _operatorVMService = operatorVMService;
            _deliveryService = deliveryService;
            _tableFilterService = tableFilterService;
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
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            EditOrderPAgeVM? viewModel = await _operatorVMService.BuildEditOrderPage(id);
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

            return RedirectToAction("OrderFound", "Operator", new { orderId = order.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AssignOrderPlace(string trackId)
        {
            string? identityName = User.Identity?.Name;
            if (identityName == null) { return RedirectToAction("AccessDenied", "Home"); }

            Place? workingPlace = await _operatorVMService.GetOperatorWorkingPlaceByIdentityAsync(identityName);
            if (workingPlace == null) { return RedirectToAction("BadReq", "Home"); }

            bool success = await _ordersService.ReassignOrderPlaceAsync(trackId, workingPlace.Id);
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

        [HttpGet]
        public async Task<IActionResult> FilterOrders(OrdersFilterRequest request)
        {
            List<Order> orders = await _tableFilterService.GetFilteredOrders(request);
            List<OrderRowVM> ordersRows = await _operatorVMService.GetOrdersTableRows(orders);

            return PartialView("_OrdersListCard", ordersRows);
        }

        [HttpGet]
        public async Task<IActionResult> FilterRedirectOrders(RedirectOrdersFilterRequest request)
        {
            string identityName = User.Identity!.Name!;

            OperatorPageVM? viewModel = await _operatorVMService.BuildDashboard("tables", identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            viewModel.OrdersList = await _tableFilterService.GetFilteredRedirectOrders(request);

            return PartialView("_RedirectOrdersList", viewModel);
        }
    }
}
