namespace Data.Models.Views
{
    public class AuthViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool Status { get; set; }
        public string Token { get; set; } = null!;

    }
}
