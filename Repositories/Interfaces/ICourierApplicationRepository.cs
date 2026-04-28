using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface ICourierApplicationRepository
    {
        Task<List<CourierApplication>> GetAllApplicationsAsync();
        Task<CourierApplication?> GetApplicationByIdAsync(int id);
        Task<CourierApplication?> GetApplicationByCourierIdAsync(int courierId);
        Task CreateApplicationAsync(CourierApplication CourierApplication);
        Task UpdateApplicationAsync(CourierApplication newData);
        Task DeleteApplicationByIdAsync(CourierApplication CourierApplication);
        Task SaveChangesAsync();
    }
}
