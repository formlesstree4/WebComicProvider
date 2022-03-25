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
        string? Synopsis);
}
