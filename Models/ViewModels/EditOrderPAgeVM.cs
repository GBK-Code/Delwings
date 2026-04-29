using Delwings.Models.Basic;

namespace Delwings.Models.ViewModels
{
    public class EditOrderPAgeVM
    {
        public Order Order { get; set; }
        public Account? Courier { get; set; }
    }
}
