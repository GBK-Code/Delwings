namespace Delwings.Models.Requests
{
    public class CreateAccountRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
