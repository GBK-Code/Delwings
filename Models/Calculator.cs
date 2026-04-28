namespace Delwings.Models
{
    public class Calculator
    {
        public double fromLat { get; set; }
        public double toLat { get; set; }

        public double fromLon { get; set; }
        public double toLon { get; set; }

        public string fromCity { get; set; }
        public string toCity { get; set; }
        public double dist { get; set; }
    }
}
