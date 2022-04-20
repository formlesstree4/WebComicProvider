using System.Text.Json.Serialization;

namespace WebComicProvider.Models.Comics
{
    public sealed class ComicPage
    {
        [JsonPropertyName("id")]
        public int PageId { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; } = string.Empty;

        [JsonPropertyName("tooltip")]
        public string? ToolTip { get; set; } = string.Empty;

        [JsonPropertyName("commentary")]
        public string? Commentary { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string? Location { get; set; } = string.Empty;

        [JsonPropertyName("published")]
        public DateTimeOffset PublishDate { get; set; }
    }
}
