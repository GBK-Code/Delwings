namespace Delwings.Models
{
    public class TrackerPageVM
    {
        public Order? Order { get; set; }
        public Place? Current { get; set; }
        public bool IsOperator { get; set; }
        public bool IsCourier { get; set; }
        public Account? Operator { get; set; } = null;
        public Account? Courier { get; set; } = null;
        public Place? OperatorPlace { get; set; }
        public List<Place>? OrderPlaces { get; set; }
    }
}
