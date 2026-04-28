using Delwings.Models;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class OrdersService
    {
        private readonly IOrdersRepository _repo;

        public OrdersService(IOrdersRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _repo.GetAllOrdersAsync();
        }
        public async Task<Order?> GetOrderByIdAsync(int id) => await _repo.GetOrderByIdAsync(id);
        public async Task<Order?> GetOrderByTrackIdAsync(string trackID) => await _repo.GetOrderByTrackIdAsync(trackID);
        public async Task<Order?> GetOrderByReceiverNumberAsync(int receiverNumber) => await _repo.GetOrderByReceiverNumberAsync(receiverNumber);
        public async Task<int> CreateOrderAsync(Order order)
        {
            await _repo.AddOrderAsync(order);
            await _repo.SaveChangesAsync();
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
            if (existing is null) return false;

            await _repo.DeleteOrderAsync(existing);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
