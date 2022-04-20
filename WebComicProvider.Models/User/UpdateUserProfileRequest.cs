namespace WebComicProvider.Models.User
{
    public sealed class UpdateUserProfileRequest
    {
        public string DisplayName { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string ProfileUrl { get; set; } = "";
    }
}
