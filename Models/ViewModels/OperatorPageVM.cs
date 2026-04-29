using Delwings.Models.Basic;

namespace Delwings.Models.ViewModels
{
    public class OperatorPageVM
    {
        public Account Me { get; set; }
        public string Tab { get; set; }
        public List<Order> OrdersList { get; set; }
        public List<Place> PlacesList { get; set; }
        public Place WorkingPlace { get; set; }
        public List<Account> AcceptedCourierList { get; set; }
        public List<CourierOrder> CourierOrders { get; set; }
    }
}
