using Delwings.Models.Basic;


namespace Delwings.Models.ViewModels.Tables
{
    public class OrderRowVM
    {
        public Order? OrderRow { get; set; }
        public Place? PlaceRow { get; set; }
        public Account? AccountRow { get; set; }
    }
}
