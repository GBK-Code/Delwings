using Delwings.Models.Enums;

namespace Delwings.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public PlaceTypes Type { get; set; }
    }
}
