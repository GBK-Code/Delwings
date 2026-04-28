using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> GetOrderByTrackIdAsync(string trackId);
        Task<Order?> GetOrderByReceiverNumberAsync(int recieverNumber);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task SaveChangesAsync();
    }
}
