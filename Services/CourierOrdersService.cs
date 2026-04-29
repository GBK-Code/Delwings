using Delwings.Models.Basic;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class CourierOrdersService
    {
        private readonly ICourierOrdersRepository _repo;

        public CourierOrdersService(ICourierOrdersRepository repo) { _repo = repo; }

        public async Task<List<CourierOrder>> GetAllCourierOrdersAsync() => await _repo.GetAllCourierOrdersAsync();
        public async Task<CourierOrder?> GetCourierOrderByIdAsync(int id) => await _repo.GetCourierOrderByIdAsync(id);
        public async Task<CourierOrder?> GetCourierOrderByCourierIdAsync(int id) => await _repo.GetCourierOrderByCourierIdAsync(id);
        public async Task<CourierOrder?> GetCourierOrderByToPlaceIdAsync(int placeId) => await _repo.GetCourierOrderByToPlaceIdAsync(placeId);
        public async Task<int> CreateCourierOrderAsync(CourierOrder order)
        {
            await _repo.CreateCourierOrderAsync(order);
            await _repo.SaveChangesAsync();

            return order.Id;
        }
        public async Task<bool> UpdateCourierOrderAsync(int id, CourierOrder newData)
        {
            var courierOrder = await _repo.GetCourierOrderByIdAsync(id);
            if (courierOrder == null) { return false; }

            courierOrder.CourierId = newData.CourierId;
            courierOrder.ToPlaceId = newData.ToPlaceId;
            courierOrder.FromPlaceId = newData.FromPlaceId;

            await _repo.UpdateCourierOrderAsync(courierOrder);
            await _repo.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteCourierOrderAsync(int id)
        {
            var orderToDelete = await _repo.GetCourierOrderByIdAsync(id);
            if (orderToDelete == null) { return false; }

            await _repo.DeleteCourierOrderAsync(orderToDelete);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourierOrderByOrderIdAsync(int orderId)
        {
            var orderToDelete = await _repo.GetCourierOrderByOrderIdAsync(orderId);
            if (orderToDelete == null) { return false; }

            await _repo.DeleteCourierOrderAsync(orderToDelete);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
