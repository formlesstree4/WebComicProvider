namespace WebComicProvider.Models.User
{
    public sealed class UserSession : UserDetails
    {
        public string Token { get; set; } = "";
    }
}
