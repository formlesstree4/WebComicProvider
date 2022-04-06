namespace WebComicProvider.Domain.Comics
{
    public sealed record ComicModel(
        int ID,
        string Name,
        string Description,
        int CreatedBy,
        DateTimeOffset CreatedOn,
        DateTimeOffset UpdatedOn,
        int Status,
        string Cover,
        int Issues,
        int Pages)
    {
        public ComicModel() : this(default, string.Empty, string.Empty, default, default, default, default, string.Empty, default, default) { }
    }
}
