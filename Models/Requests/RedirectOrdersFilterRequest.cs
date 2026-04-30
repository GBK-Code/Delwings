using Delwings.Models.Enums;

namespace Delwings.Models.Requests
{
    public class RedirectOrdersFilterRequest
    {
        public string? TrackId { get; set; }
        public string? Address { get; set; }
        public bool Ordinary { get; set; }
        public bool Insured { get; set; }
        public bool Express { get; set;  }
    }
}
