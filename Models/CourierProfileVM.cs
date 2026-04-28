using Delwings.Models.Enums;

namespace Delwings.Models
{
    public class CourierProfileVM
    {
        public Account Account { get; set; }
        public string Tab { get; set; }
        public CourierStatuses Status {  get; set; }
        public List<Place> FromPlacesList { get; set; }
        public List<Place> ToPlacesList { get; set; }
        public List<CourierOrder> CourierOrders { get; set; }
    }
}
