namespace WebComicProvider.Domain.Users
{
    public record RoleModel(int ID, string Name, string Description)
    {
        public RoleModel() : this(default, string.Empty, string.Empty) { }
    }
}
