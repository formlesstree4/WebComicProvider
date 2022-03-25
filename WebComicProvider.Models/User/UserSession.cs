namespace WebComicProvider.Models.User
{
    public sealed class UserSession
    {
        public string Username { get; set; } = "";
        public string Token { get; set; } = "";
        public string Email { get; set; } = "";
        public List<string> UserRoles { get; set; } = new();
    }
}
