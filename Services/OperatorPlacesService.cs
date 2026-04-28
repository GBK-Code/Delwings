using Delwings.Models;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class OperatorPlacesService
    {
        private readonly IOperatorPlacesRepository _repository;

        public OperatorPlacesService(IOperatorPlacesRepository repository) { _repository = repository; }

        public async Task<List<OperatorPlace>> GetAllOperatorPlacesAsync() => await _repository.GetAllOperatorPlacesAsync();
        public async Task<OperatorPlace?> GetOperatorPlaceByIdAsync(int id) => await _repository.GetOperatorPlaceByIdAsync(id);
        public async Task<OperatorPlace?> GetOperatorPlaceByOperatorIdAsync(int id) => await _repository.GetOperatorPlaceByOperatorIdAsync(id);
        public async Task<OperatorPlace?> GetOperatorPlaceByPlaceIdAsync(int id) => await _repository.GetOperatorPlaceByPlaceIdAsync(id);
        public async Task<OperatorPlace> BuildOperatorPlace(int operatorId, int placeId)
        {
            var newPlace = new OperatorPlace
            {
                OperatorId = operatorId,
                PlaceId = placeId
            };

            return newPlace;
        }
        public async Task<int> CreateOperatorPlace(int operatorId, int placeId)
        {
            OperatorPlace newPlace = await BuildOperatorPlace(operatorId, placeId);

            await _repository.CreateOperatorPlaceAsync(newPlace);
            await _repository.SaveChangesAsync();
            return newPlace.Id;
        }
        public async Task<bool> UpdateOperatorPlace(int id, OperatorPlace newData)
        { 
            var existing = await _repository.GetOperatorPlaceByIdAsync(id);
            if (existing == null) { return false; }

            existing.OperatorId = newData.OperatorId;
            existing.PlaceId = newData.PlaceId;

            await _repository.UpdateOperatorPlace(existing);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOperatorPlace(int id)
        {
            var existing = await _repository.GetOperatorPlaceByIdAsync(id);
            if (existing == null) { return false; }

            await _repository.DeleteOperatorPlace(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
