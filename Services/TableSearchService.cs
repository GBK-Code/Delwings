using Delwings.Models.Basic;

namespace Delwings.Services
{
    public class TableSearchService
    {
        private readonly OrdersService _ordersService;
        private readonly PlaceService _placeService;

        public TableSearchService(OrdersService ordersService, PlaceService placeService)
        {
            _ordersService = ordersService;
            _placeService = placeService;
        }

        public async Task<List<Order>> SearchOrdersByDate(string from, string to)
        {
            List<Order> orders = await _ordersService.GetAllOrdersAsync();
            List<Order> filtered = new List<Order>();

            DateOnly fromDate = DateOnly.Parse(from);
            DateOnly toDate = DateOnly.Parse(to);

            foreach (Order order in orders)
            {
                if (order.Date == null) { continue; }
                DateOnly orderDate = DateOnly.Parse(order.Date);

                if (orderDate >= fromDate && orderDate <=  toDate)
                {
                    filtered.Add(order);
                }
            }

            return filtered;
        }


        public async Task<List<Order>> SearchOrderByTrackSubString(List<Order> orders, string substring)
        {
            List<Order> filtered = new List<Order>();

            foreach (Order order in orders)
            {
                if (order.TrackId == null) { continue; }

                if (order.TrackId.ToLower().Contains(substring.ToLower()))
                {
                    filtered.Add(order);
                }
            }

            return filtered;
        }

        public async Task<List<Order>> SearchOrderByAddressSubString(List<Order> orders, string substring)
        {
            List<Order> filtered = new List<Order>();

            foreach (Order order in orders)
            {
                Place? orderPlace = await _placeService.GetPlaceByIdAsync(order.CurrentLocationId);
                if (orderPlace == null) { continue; }

                string placeString = $"{orderPlace.Country}, {orderPlace.City}, {orderPlace.Address}";

                if (placeString.ToLower().Contains(substring.ToLower()))
                {
                    filtered.Add(order);
                }
            }

            return filtered;
        }

        public async Task<List<Place>> SearchPlacesByCountrySubstring(List<Place> places, string substring)
        {
            List<Place> filtered = new List<Place>();

            foreach (Place place in places)
            {
                if (place.Country == null) { continue; }

                if (place.Country.ToLower().Contains(substring.ToLower()))
                {
                    filtered.Add(place);
                }
            }

            return filtered;
        }

        public async Task<List<Place>> SearchPlacesByCitySubstring(List<Place> places, string substring)
        {
            List<Place> filtered = new List<Place>();

            foreach (Place place in places)
            {
                if (place.City == null) { continue; }

                if (place.City.ToLower().Contains(substring.ToLower()))
                {
                    filtered.Add(place);
                }
            }

            return filtered;
        }

        public async Task<List<Place>> SearchPlacesByAddressSubstring(List<Place> places, string substring)
        {
            List<Place> filtered = new List<Place>();

            foreach (Place place in places)
            {
                if (place.Address == null) { continue; }

                if (place.Address.ToLower().Contains(substring.ToLower()))
                {
                    filtered.Add(place);
                }
            }

            return filtered;
        }
    }
}
