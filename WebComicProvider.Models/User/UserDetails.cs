namespace WebComicProvider.Models.User
{
    public class UserDetails
    {
        public int UserId { get; set; } = 0;
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public List<string> UserRoles { get; set; } = new();
    }
}
