using Delwings.Models.Basic;

namespace Delwings.Models.ViewModels
{
    public class UserProfileVM
    {
        public Account Me { get; set; }
        public List<Order> MyOrdersList { get; set; }
        public List<Account> CourierList { get; set; }
    }
}
