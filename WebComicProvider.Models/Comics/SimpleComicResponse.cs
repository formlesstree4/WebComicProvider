using System.Text.Json.Serialization;

namespace WebComicProvider.Models.Comics
{
    public sealed class SimpleComicResponse
    {
        [JsonPropertyName("id")]
        public int ComicId { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("comicName")]
        public string ComicName { get; set; } = string.Empty;

        [JsonPropertyName("synopsis")]
        public string Synopsis { get; set; } = string.Empty;

        [JsonPropertyName("issueCount")]
        public int Issues { get; set; }

        [JsonPropertyName("pageCount")]
        public int TotalPages { get; set; }

        [JsonPropertyName("cover")]
        public string Cover { get; set; } = string.Empty;

        [JsonPropertyName("created")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonPropertyName("updated")]
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
