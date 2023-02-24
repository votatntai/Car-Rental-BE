namespace Data.Models.Update
{
    public class UserUpdateModel
    {
        public string? Name { get; set; } = null!;

        public string? Phone { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string? Gender { get; set; } = null!;

        public bool? Status { get; set; } = null!;
    }
}
