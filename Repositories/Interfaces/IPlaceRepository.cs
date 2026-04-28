using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface IPlaceRepository
    {
        Task<List<Place>> GetAllPlacesAsync();
        Task<Place?> GetPlaceByIdAsync(int id);
        Task CreatePlaceAsync(Place place);
        Task UpdatePlaceAsync(Place place);
        Task DeletePlaceAsync(Place place);
        Task SaveChangesAsync();
    }
}
