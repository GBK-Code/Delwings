using Delwings.Models;
using Delwings.Models.Requests;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class OrdersService
    {
        private readonly IOrdersRepository _repo;
        private readonly OrderTokensGenerator _tokenGenerator;
        private readonly OrdersHistoryService _ordersHistoryService;
        private readonly CourierOrdersService _courierOrdersService;

        public OrdersService (
            IOrdersRepository repo, 
            OrderTokensGenerator tokensGenerator, 
            OrdersHistoryService historyService,
            CourierOrdersService courierOrdersService
            )
        {
            _repo = repo;
            _tokenGenerator = tokensGenerator;
            _ordersHistoryService = historyService;
            _courierOrdersService = courierOrdersService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _repo.GetAllOrdersAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id) => await _repo.GetOrderByIdAsync(id);
        public async Task<Order?> GetOrderByTrackIdAsync(string trackID) => await _repo.GetOrderByTrackIdAsync(trackID);
        public async Task<Order?> GetOrderByReceiverNumberAsync(int receiverNumber) => await _repo.GetOrderByReceiverNumberAsync(receiverNumber);

        public Order BuildOrder(OrderRequest request)
        {
            Order order = new Order()
            {
                Id = request.Id,
                OrderType = request.OrderType,
                Date = request.Date,
                Time = request.Time,
                TrackId = request.TrackId,
                ReceiverNumber = request.ReceiverNumber,
                SenderId = request.SenderId,
                IsCarried = false,
                ReceiverContact = request.Contact,
                Destination = request.Destination,
                CurrentLocationId = request.CurrentPlaceId,
                CourierId = request.CourierId,
                InsuranceCompany = request.InsuranceCompany,
                InsurancePrice = request.InsurancePrice
            };

            return order;
        }

        public async Task<bool> ReassignOrderPlaceAsync(string trackId, int placeId)
        {
            Order? order = await GetOrderByTrackIdAsync(trackId);
            if (order == null) { return false; }

            order.CurrentLocationId = placeId;

            var success = await UpdateOrderAsync(order.Id, order);
            if (!success) { return false; }

            return true;
        }

        public async Task<int> CreateOrderAsync(OrderRequest request)
        {
            Order order = BuildOrder(request);
            order.TrackId = _tokenGenerator.GenerateTrackId();
            order.ReceiverNumber = _tokenGenerator.GenerateReceiverNumber();

            await _repo.AddOrderAsync(order);
            await _repo.SaveChangesAsync();

            await _ordersHistoryService.CreateRecordAsync(new OrdersHistory
            {
                OrderId = order.Id,
                PlaceId = order.CurrentLocationId
            });

            return order.Id;
        }
        public async Task<bool> UpdateOrderAsync(int id, Order updatedOrder)
        {
            var existingOrder = await _repo.GetOrderByIdAsync(id);
            if (existingOrder == null) { return false; }

            existingOrder.Date = updatedOrder.Date;
            existingOrder.Time = updatedOrder.Time;
            existingOrder.SenderId = updatedOrder.SenderId;
            existingOrder.TrackId = updatedOrder.TrackId;
            existingOrder.CurrentLocationId = updatedOrder.CurrentLocationId;
            existingOrder.ReceiverContact = updatedOrder.ReceiverContact;
            existingOrder.CourierId = updatedOrder.CourierId;
            existingOrder.InsuranceCompany = updatedOrder.InsuranceCompany;
            existingOrder.InsurancePrice = updatedOrder.InsurancePrice;

            await _repo.UpdateOrderAsync(existingOrder);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var existing = await _repo.GetOrderByIdAsync(id);
            if (existing == null) { return false; }

            await _repo.DeleteOrderAsync(existing);
            await _repo.SaveChangesAsync();

            await _courierOrdersService.DeleteCourierOrderByOrderIdAsync(id);

            return true;
        }
    }
}
