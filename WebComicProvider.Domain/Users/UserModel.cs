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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public UserModel() : this(default, default, default, default, default, default, default, default) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    };
}
