using Delwings.Repositories.Interfaces;

namespace Delwings.Services
{
    public class DataEraserService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IPlaceRepository _placeRepository;

        public DataEraserService(IOrdersRepository ordersRepository, IPlaceRepository placeRepository)
        {
            _ordersRepository = ordersRepository;
            _placeRepository = placeRepository;
        }

        public async Task EraseOrders()
        {
            await _ordersRepository.ClearTable();
            await _ordersRepository.SaveChangesAsync();
        }

        public async Task ErasePlaces()
        {
            await _placeRepository.ClearPlaces();
            await _placeRepository.SaveChangesAsync();
        }

    }
}
