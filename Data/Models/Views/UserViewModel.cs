namespace Data.Models.Views
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string Role { get; set; } = null!;

        public bool Status { get; set; }
    }
}
