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

        private async Task<List<Order>> FilterOrderType(List<Order> notFiltered, bool ordinary, bool insured, bool express)
        {
            List<Order> filtered = notFiltered.Where
                (
                    ord => (ord.OrderType == OrderTypes.Ordinary && ordinary) ||
                            (ord.OrderType == OrderTypes.Express && express) ||
                            (ord.OrderType == OrderTypes.Insured && insured)
                ).ToList();

            return filtered;
        }

        public TableFilterService(TableSearchService tableSearchService, OrdersService ordersService)
        {
            _tableSearchService = tableSearchService;
            _ordersService = ordersService;
        }

        public async Task<List<Order>> GetFilteredOrders(OrdersFilterRequest request)
        {
            List<Order> filtered = await _ordersService.GetAllOrdersAsync();

            if (request.From != null && request.To != null)
            {
                filtered = await _tableSearchService.SearchOrdersByDate(request.From, request.To);
            }

            filtered = await FilterOrderType(filtered, request.Ordinary, request.Insured, request.Express);

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
            bool insuredCheck = request.Insured;
            bool ordinaryCheck = request.Ordinary;
            bool expressCheck = request.Express;

            List<Order> filtered = await _ordersService.GetAllOrdersAsync();
            filtered = await FilterOrderType(filtered, ordinaryCheck, insuredCheck, expressCheck);

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
