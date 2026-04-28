namespace Delwings.Models
{
    public class AdminPageVM
    {
        public Account Me { get; set; }
        public string Tab { get; set; }
        public List<Account> OperatorsList { get; set; }
        public List<Place> PlacesList { get; set; }
        public List<OperatorPlace> OperatorPlacesList { get; set; }
        public List<CourierApplication> CouriersApplicationsList { get; set; }
    }
}
