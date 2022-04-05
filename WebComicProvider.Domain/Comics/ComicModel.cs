namespace WebComicProvider.Domain.Comics
{
    internal sealed record ComicModel(
        int ID,
        string Name,
        string Description,
        int CreatedBy,
        DateTimeOffset CreatedOn,
        DateTimeOffset UpdatedOn,
        int Status)
    {
        public ComicModel() : this(default, string.Empty, string.Empty, default, default, default, default) { }
    }
}
