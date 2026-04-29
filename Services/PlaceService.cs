using Delwings.Models.Basic;
using Delwings.Models.Requests;
using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class PlaceService
    {
        private readonly IPlaceRepository _repository;

        public PlaceService(IPlaceRepository repository) { _repository = repository; }

        public async Task<List<Place>> GetAllPlacesAsync() => await _repository.GetAllPlacesAsync();
        public async Task<Place?> GetPlaceByIdAsync(int id) => await _repository.GetPlaceByIdAsync(id);

        public async Task<int> CreatePlaceAsync(CreatePlaceRequest request)
        {
            Place place = new Place
            {
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Contact = request.Contact,
                Type = request.PlaceType
            };

            return await CreatePlaceAsync(place);
        }

        public async Task<int> CreatePlaceAsync(Place place)
        {
            await _repository.CreatePlaceAsync(place);
            await _repository.SaveChangesAsync();
            return place.Id;
        }

        public async Task<bool> UpdatePlaceAsync(int id, Place newData)
        {
            var existingPlace = await _repository.GetPlaceByIdAsync(id);
            if (existingPlace == null) { return false; }

            existingPlace.Address = newData.Address;
            existingPlace.City = newData.City;
            existingPlace.Contact = newData.Contact;
            existingPlace.Country = newData.Country;
            existingPlace.Type = newData.Type;

            await _repository.UpdatePlaceAsync(existingPlace);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlaceByIdAsync(int id)
        {
            var placeToDelete = await _repository.GetPlaceByIdAsync(id);
            if (placeToDelete == null) { return false; }

            await _repository.DeletePlaceAsync(placeToDelete);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
