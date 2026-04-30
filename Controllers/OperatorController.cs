using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.Requests;
using Delwings.Models.ViewModels;
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
        private readonly TableSearchService _tableSearchService;
        
        public OperatorController 
            (
                OrdersService ordersService,
                OperatorVMService operatorVMService,
                DeliveryService deliveryService,
                TableSearchService tableSearchService
            )
        {
            _ordersService = ordersService;
            _operatorVMService = operatorVMService;
            _deliveryService = deliveryService;
            _tableSearchService = tableSearchService;
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

            return RedirectToAction("OrderFound", "Operator", new { orderId = order.Id });
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

        [HttpGet]
        public async Task<IActionResult> SearchOrdersByDate(OrdersFilterRequest request)
        {
            string? identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            List<Order> filtered = await _ordersService.GetAllOrdersAsync();

            if (request.From != null && request.To != null)
            {
                filtered = await _tableSearchService.SearchOrdersByDate(request.From, request.To);
            }

            filtered = filtered.Where
                (
                    ord => (ord.OrderType == OrderTypes.Ordinary && request.Ordinary) ||
                            (ord.OrderType == OrderTypes.Express && request.Express) ||
                            (ord.OrderType == OrderTypes.Insured && request.Insured)
                ).ToList();

            if (request.Sorted)
            {
                if (request.Descending) { filtered = filtered.OrderByDescending(ord => ord.Date).ToList(); }
                else { filtered = filtered.OrderBy(ord => ord.Date).ToList(); }
            }
           

            OperatorPageVM? viewModel = await _operatorVMService.BuildDashboard("tables", identityName);
            if (viewModel == null) { return RedirectToAction("AccessDenied", "Home"); }

            viewModel.OrdersList = filtered;

            return PartialView("_OrdersListCard", viewModel);
        }
    }
}
