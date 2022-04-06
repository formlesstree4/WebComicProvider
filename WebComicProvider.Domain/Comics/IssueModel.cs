namespace WebComicProvider.Domain.Comics
{
    public sealed record IssueModel(
        int ID,
        int ComicID,
        string Name,
        DateTimeOffset CreatedOn,
        DateTimeOffset UpdatedOn,
        int Status,
        DateTimeOffset? ReleaseDate,
        string? Synopsis,
        int Number,
        int Pages)
    {
        public IssueModel() : this(default, default, string.Empty, default, default, default, default, default, default, default) { }
    }
}
