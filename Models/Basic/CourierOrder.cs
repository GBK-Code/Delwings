namespace Delwings.Models.Basic
{
    public class CourierOrder
    {
        public int Id { get; set; }
        public int CourierId { get; set; }
        public int FromPlaceId { get; set; }
        public int ToPlaceId { get; set; }
        public int OrderId { get; set; }
    }
}
