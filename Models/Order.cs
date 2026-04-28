using Delwings.Models.Enums;

namespace Delwings.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderTypes OrderType { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string TrackId { get; set; }
        public int ReceiverNumber { get; set; }
        public int SenderId { get; set; }
        public bool IsCarried { get; set; } = false;
        public string ReceiverContact { get; set; }
        public string Destination { get; set; }
        public int CurrentLocationId { get; set; }
        public int? CourierId { get; set; }
        public string? InsuranceCompany { get; set; }
        public int? InsurancePrice { get; set; }

        public void Print() { }
    }
}
