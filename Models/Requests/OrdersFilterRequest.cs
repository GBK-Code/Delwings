namespace Delwings.Models.Requests
{
    public class OrdersFilterRequest
    {
        // Date
        public string? From { get; set; }
        public string? To { get; set; }
        public bool Ordinary { get; set; }
        public bool Express { get; set; }
        public bool Insured { get; set; }
        public bool Sorted { get; set; }
        public bool Descending { get; set; }
    }
}
