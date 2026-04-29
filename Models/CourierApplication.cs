using Delwings.Models.Enums;

namespace Delwings.Models
{
    public class CourierApplication
    {
        public int Id { get; set; }

        public int CourierId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public CourierStatuses Status { get; set; }
    }
}
