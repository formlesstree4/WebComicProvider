namespace WebComicProvider.Domain.Users
{
    public record UserModel(
        int ID,
        string Username,
        string Display,
        byte[] Password,
        byte[] Salt,
        string EmailAddress,
        int Iterations,
        string ProfileUrl)
    {
        public UserModel() : this(default, string.Empty, string.Empty, Array.Empty<byte>(), Array.Empty<byte>(), string.Empty, default, string.Empty) { }
    };
}
