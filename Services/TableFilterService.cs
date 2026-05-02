using Azure.Core;
using Delwings.Models.Basic;
using Delwings.Models.Enums;
using Delwings.Models.Requests;


namespace Delwings.Services
{
    public class TableFilterService
    {
        private readonly TableSearchService _tableSearchService;
        private readonly OrdersService _ordersService;
        private readonly PlaceService _placeService;

        public TableFilterService(TableSearchService tableSearchService, OrdersService ordersService, PlaceService placeService)
        {
            _tableSearchService = tableSearchService;
            _ordersService = ordersService;
            _placeService = placeService;
        }
        private async Task<List<Order>> FilterOrdersByType(List<Order> notFiltered, bool ordinary, bool insured, bool express)
        {
            List<Order> filtered = notFiltered.Where
                (
                    ord => (ord.OrderType == OrderTypes.Ordinary && ordinary) ||
                            (ord.OrderType == OrderTypes.Express && express) ||
                            (ord.OrderType == OrderTypes.Insured && insured)
                ).ToList();

            return filtered;
        }

        private async Task<List<Order>> FilterOrdersByPlaceType(List<Order> notFiltered, bool accept, bool sorting, bool pickup)
        {
            List<Order> filtered = new List<Order>();
            List<Place> places = await _placeService.GetAllPlacesAsync();

            foreach (Order order in notFiltered)
            {
                Place? orderPlace = places.Where(plc => plc.Id == order.CurrentLocationId).FirstOrDefault();
                if (orderPlace == null) { continue; }

                if ( (orderPlace.Type == PlaceTypes.AcceptPoint && accept) ||
                     (orderPlace.Type == PlaceTypes.SortingPoint && sorting) ||
                     (orderPlace.Type == PlaceTypes.PickUpPoint && pickup) )
                {
                    filtered.Add(order);
                }
            }

            return filtered;
        }

        public async Task<List<Order>> GetFilteredOrders(OrdersFilterRequest request)
        {
            List<Order> filtered = await _ordersService.GetAllOrdersAsync();

            if (request.From != null && request.To != null)
            {
                filtered = await _tableSearchService.SearchOrdersByDate(request.From, request.To);
            }

            filtered = await FilterOrdersByType(filtered, request.Ordinary, request.Insured, request.Express);

            if (request.Sorted)
            {
                if (request.Descending) { filtered = filtered.OrderByDescending(ord => DateOnly.Parse(ord.Date)).ToList(); }
                else { filtered = filtered.OrderBy(ord => DateOnly.Parse(ord.Date)).ToList(); }
            }

            return filtered;
        }

        public async Task<List<Order>> GetFilteredRedirectOrders(RedirectOrdersFilterRequest request)
        {
            string? trackId = request.TrackId;
            string? address = request.Address;
            bool acceptCheck = request.Accept;
            bool sortingCheck = request.Sorting;
            bool pickupCheck = request.PickUp;

            List<Order> filtered = await _ordersService.GetAllOrdersAsync();
            filtered = await FilterOrdersByPlaceType(filtered, acceptCheck, sortingCheck, pickupCheck);

            if (trackId != null)
            {
                filtered = await _tableSearchService.SearchOrderByTrackSubString(filtered, trackId);
            }

            if (address != null)
            {
                filtered = await _tableSearchService.SearchOrderByAddressSubString(filtered, address);
            }

            return filtered;
        }
    }
}
