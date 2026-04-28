using Delwings.Models;

namespace Delwings.Repositories.Interfaces
{
    public interface IOperatorPlacesRepository
    {
        Task<List<OperatorPlace>> GetAllOperatorPlacesAsync();
        Task<OperatorPlace> BuildOperatorPlace(int operatorId, int placeId);
        Task<OperatorPlace?> GetOperatorPlaceByIdAsync(int id);
        Task<OperatorPlace?> GetOperatorPlaceByOperatorIdAsync(int operatorId);
        Task<OperatorPlace?> GetOperatorPlaceByPlaceIdAsync(int placeId);
        Task CreateOperatorPlaceAsync(OperatorPlace place);
        Task UpdateOperatorPlace(OperatorPlace newData);
        Task DeleteOperatorPlace(OperatorPlace place);
        Task SaveChangesAsync();
    }
}
