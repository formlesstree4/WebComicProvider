namespace WebComicProvider.Models.Comics
{
    public sealed class ComicPage
    {
        public int PageId { get; set; }
        public int PageNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ToolTip { get; set; } = string.Empty;
        public string Commentary { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
