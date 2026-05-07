using Delwings.Models.Basic;

namespace Delwings.Models.ViewModels
{
    public class HeadPageVM
    {
        public Account Me { get; set; }
        public List<Order> Orders { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Account> AdminAccounts { get; set; }
        public List<Place> Places { get; set; }
        public string Tab { get; set; }
        public DevToolsModel DeveloperModel { get; set; }
    }
}
