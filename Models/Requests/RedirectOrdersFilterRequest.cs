namespace Delwings.Models.Requests
{
    public class RedirectOrdersFilterRequest
    {
        public string? TrackId { get; set; }
        public string? Address { get; set; }
        public bool Accept { get; set; }
        public bool Sorting { get; set; }
        public bool PickUp { get; set;  }
    }
}
