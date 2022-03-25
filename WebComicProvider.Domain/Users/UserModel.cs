namespace WebComicProvider.Domain.Users
{
    public record UserModel(
        int ID,
        string Username,
        string Display,
        byte[] Password,
        byte[] Salt,
        string Email,
        int Iterations)
    {
        public UserModel() : this(default, default, default, default, default, default, default) { }
    };
}
