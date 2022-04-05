﻿namespace WebComicProvider.Domain.Comics
{
    public sealed record PageModel(
        int ID,
        int IssueID,
        string? Title,
        string? ToolTip,
        string? Commentary,
        int Status,
        DateTimeOffset ReleaseDate)
    {
        public PageModel() : this(default, default, default, default, default, default, default) { }
    }
}
