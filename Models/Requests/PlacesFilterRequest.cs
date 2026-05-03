namespace Delwings.Models.Requests
{
    public class PlacesFilterRequest
    {
        public bool IsAccept { get; set; }
        public bool IsSorting { get; set; }
        public bool IsPickup { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
