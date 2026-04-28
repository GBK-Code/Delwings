using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface ICourierOrdersRepository
    {
        Task<List<CourierOrder>> GetAllCourierOrdersAsync();
        Task<CourierOrder?> GetCourierOrderByIdAsync(int id);
        Task<CourierOrder?> GetCourierOrderByCourierIdAsync(int id);
        Task<CourierOrder?> GetCourierOrderByToPlaceIdAsync(int placeId);
        Task CreateCourierOrderAsync(CourierOrder order);
        Task UpdateCourierOrderAsync(CourierOrder order);
        Task DeleteCourierOrderAsync(CourierOrder order);
        Task SaveChangesAsync();
    }
}
