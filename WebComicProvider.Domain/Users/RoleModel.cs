namespace WebComicProvider.Domain.Users
{
    public record RoleModel(int ID, string Name, string Description)
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public RoleModel() : this(default, default, default) { }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
