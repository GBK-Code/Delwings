using Delwings.Models.Basic;

namespace Delwings.Services
{
    public class TableSearchService
    {
        private readonly OrdersService _ordersService;

        public TableSearchService(OrdersService ordersService)
        {
            _ordersService = ordersService;
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
    }
}
