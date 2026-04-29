using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class OrdersHistoryService
    {
        private readonly IOrdersHistoryRepository _repo;

        public OrdersHistoryService(IOrdersHistoryRepository ordersHistoryRepository)
        {
            _repo = ordersHistoryRepository;
        }

        public async Task<List<OrdersHistory>> GetAllHistoryAsync() => await _repo.GetAllHistoryAsync();
        public async Task<OrdersHistory?> GetRecordByIdAsync(int id) => await _repo.GetRecordByIdAsync(id);
        public async Task<int> CreateRecordAsync(OrdersHistory orderRecord)
        {
            await _repo.CreateRecordAsync(orderRecord);
            await _repo.SaveChangesAsync();
            return orderRecord.Id;
        }
        public async Task<bool> UpdateRecordAsync(int id, OrdersHistory newData)
        {
            var existing = await _repo.GetRecordByIdAsync(id);
            if (existing == null) { return false; }

            existing.OrderId = newData.OrderId;
            existing.PlaceId = newData.PlaceId;

            await _repo.UpdateRecordAsync(existing);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleterecordByIdAsync(int id)
        {
            var existing = await _repo.GetRecordByIdAsync(id);
            if (existing == null) { return false; }

            await _repo.DeleteRecordAsync(existing);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
