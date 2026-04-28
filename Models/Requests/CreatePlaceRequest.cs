using Delwings.Models.Enums;

namespace Delwings.Models.Requests
{
    public class CreatePlaceRequest
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public PlaceTypes PlaceType { get; set; }
    }
}
