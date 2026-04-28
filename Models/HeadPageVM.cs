namespace Delwings.Models
{
    public class HeadPageVM
    {
        public Account Me { get; set; }
        public List<Account> AdminAccounts { get; set; }
        public List<Place> Places { get; set; }
        public string Tab { get; set; }
    }
}
