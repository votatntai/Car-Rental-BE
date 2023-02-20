namespace Data.Models.Views
{
    public class AuthViewModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = null!;
        public bool Status { get; set; }
    }
}
