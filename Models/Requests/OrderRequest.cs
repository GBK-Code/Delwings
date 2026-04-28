using Delwings.Models.Enums;

namespace Delwings.Models.Requests
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public int SenderId { get; set; }
        public string? TrackId { get; set; }
        public string? Destination { get; set; }
        public string? Contact { get; set; }
        public int CurrentPlaceId { get; set; }
        public OrderTypes OrderType { get; set; }
        public int? CourierId { get; set; }
        public string? InsuranceCompany { get; set; }
        public int? InsurancePrice { get; set; }
    }
}
