using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface IOrdersHistoryRepository
    {
        Task<List<OrdersHistory>> GetAllHistoryAsync();
        Task<OrdersHistory?> GetRecordByIdAsync(int id);
        Task CreateRecordAsync(OrdersHistory orderRecord);
        Task UpdateRecordAsync(OrdersHistory orderRecord);
        Task DeleteRecordAsync(OrdersHistory orderRecord);
        Task SaveChangesAsync();
    }
}
