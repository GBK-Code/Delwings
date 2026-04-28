using Delwings.Models;
using Delwings.Models.Enums;
using Delwings.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Delwings.Controllers
{
    [Authorize(Roles = "Operator", AuthenticationSchemes = "Cookies")]
    public class OperatorController: Controller
    {
        private readonly AccountService _accountService;
        private readonly OrdersService _ordersService;
        private readonly OperatorPlacesService _operatorPlacesService;
        private readonly PlaceService _placeService;
        private readonly OrdersHistoryService _ordersHistoryService;
        private readonly CourierOrdersService _courierOrdersService;
        
        public OperatorController(
            AccountService accountsService,
            OrdersService ordersService,
            OperatorPlacesService operatorPlacesService,
            PlaceService placeService,
            OrdersHistoryService ordersHistoryService,
            CourierOrdersService courierOrdersService)
        {
            _accountService = accountsService;
            _ordersService = ordersService;
            _operatorPlacesService = operatorPlacesService;
            _placeService = placeService;
            _ordersHistoryService = ordersHistoryService;
            _courierOrdersService = courierOrdersService;
        }

        private RedirectToActionResult Reload(string tab)
        {
            return RedirectToAction("Dashboard", "Operator", new { tab });
        }

        public async Task<IActionResult> Dashboard(string tab)
        {
            var identityName = User.Identity?.Name;
            if (string.IsNullOrEmpty(identityName)) { return RedirectToAction("AccessDenied", "Home"); }

            var courierList = await _accountService.GetAllAccountsAsync();
            courierList = courierList.Where(user => user.Role == AccountRoles.Courier).ToList();

            var oper = await _accountService.GetAccountByLoginAsync(identityName);
            if (oper == null) { return RedirectToAction("AccessDenied", "Home"); }

            var orders = await _ordersService.GetAllOrdersAsync();

            var operatorPlace = await _operatorPlacesService.GetOperatorPlaceByOperatorIdAsync(oper.Id);
            if (operatorPlace == null) { return RedirectToAction("AccessDenied", "Home"); }

            var places = await _placeService.GetAllPlacesAsync();

            var workingPlace = await _placeService.GetPlaceByIdAsync(operatorPlace.PlaceId);
            if (workingPlace == null) { return RedirectToAction("AccessDenied", "Home"); }

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

            return View(viewModel);
        }
        public async Task<IActionResult> OrderFound(int orderId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null) { return RedirectToAction("BadReq", "Home"); }

            return View(order);
        }
        public async Task<IActionResult> EditOrder(
                int id,
                string date,
                string time,
                int senderId,
                string trackId,
                string destination,
                string contact,
                int currentPlaceId,
                string orderType,
                int? courierId,
                string? insuranceCompany,
                int? insurancePrice
        )
        {
            if (!Enum.TryParse<OrderTypes>(orderType, out var type)) { return RedirectToAction("BadReq", "Home"); }

            var orderData = new Order
            {
                Id = id,
                Date = date,
                Time = time,
                TrackId = trackId,
                SenderId = senderId,
                Destination = destination,
                CurrentLocationId = currentPlaceId,
                ReceiverContact = contact,
                CourierId = courierId,
                InsuranceCompany = insuranceCompany,
                InsurancePrice = insurancePrice,
                OrderType = type
            };

            Account? courier = null;

            if (courierId != null)
            {
                courier = await _accountService.GetAccountByIdAsync(courierId.Value);
                if (courier == null) { return RedirectToAction("AccessDenied", "Home"); }
            }

            var vm = new EditOrderPAgeVM
            {
                Order = orderData,
                Courier = courier
            };

            return View(vm);
        }

        public async Task<IActionResult> SubmitEdit(
                int id,
                string date,
                string time,
                int senderId,
                string trackId,
                string contact,
                int currentPlaceId,
                int? courierId,
                int? insurancePrice,
                string? insuranceCompany
        )
        {   
            var orderData = new Order
            {
                Id = id,
                Date = date,
                Time = time,
                TrackId = trackId,
                SenderId = senderId,
                CurrentLocationId = currentPlaceId,
                ReceiverContact = contact,
                CourierId = courierId,
                InsuranceCompany = insuranceCompany,
                InsurancePrice = insurancePrice
            };

            await _ordersService.UpdateOrderAsync(id, orderData);

            return Reload("orders");
        }

        public async Task<IActionResult> AddOrder(
            string date,
            string time,
            string orderType,
            int senderId,
            string? trackId,
            string destination,
            string contact,
            int? courierId,
            string? insuranceCompany,
            int? insurancePrice,
            int currentPlaceId
        )
        {
            orderType = orderType.Replace("Order", "");
            OrderTypes type;
            bool success = Enum.TryParse(orderType, out type);
            if (success == false) { RedirectToAction("BadReq", "Home"); }

            if (trackId == null)
            {
                byte[] hashData = new byte[10];
                RandomNumberGenerator.Fill(hashData);
                trackId = Convert.ToHexString(SHA256.HashData(hashData)).Substring(0, 10);
            }

            Random rng = new Random();

            int orderId = await _ordersService.CreateOrderAsync(new Order
            {
                OrderType = type,
                Date = date,
                Time = time,
                TrackId = trackId,
                SenderId = senderId,
                Destination = destination,
                CurrentLocationId = currentPlaceId,
                ReceiverContact = contact,
                CourierId = (type == OrderTypes.Express) ? courierId : null,
                InsuranceCompany = (type == OrderTypes.Insured) ? insuranceCompany : null,
                InsurancePrice = (type == OrderTypes.Insured) ? insurancePrice : null,
                ReceiverNumber = rng.Next(1000000)
            }
            );

            await _ordersHistoryService.CreateRecordAsync(new OrdersHistory
            {
                OrderId = orderId,
                PlaceId = currentPlaceId,
            });

            return Reload("orders");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var courierOrders = await _courierOrdersService.GetAllCourierOrdersAsync();
            var courierOrderToDelete = courierOrders.Where(ord => ord.OrderId == id).FirstOrDefault();
            if (courierOrderToDelete != null)
            {
                await _courierOrdersService.DeleteCourierOrderAsync(courierOrderToDelete.Id);
            }
            
            await _ordersService.DeleteOrderAsync(id);
            return Reload("orders");
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

            return Reload("orders");
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

            return Reload("redirection");
        }

        public async Task<IActionResult> ChooseTab(string tab)
        {
            return Reload(tab);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}
