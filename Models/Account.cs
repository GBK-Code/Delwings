using Delwings.Models.Enums;

namespace Delwings.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public AccountRoles Role { get; set; }

    }
}
