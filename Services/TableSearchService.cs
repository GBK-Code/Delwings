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

                if (order.TrackId.Contains(substring))
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

                if (placeString.Contains(substring))
                {
                    filtered.Add(order);
                }
            }

            return filtered;
        }
    }
}
