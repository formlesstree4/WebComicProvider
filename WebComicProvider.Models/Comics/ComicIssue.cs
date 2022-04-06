using System.Text.Json.Serialization;

namespace WebComicProvider.Models.Comics
{
    public sealed class ComicIssue
    {
        [JsonPropertyName("id")]
        public int IssueId { get; set; }

        [JsonPropertyName("number")]
        public int IssueNumber { get; set; }

        [JsonPropertyName("name")]
        public string IssueName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string IssueDescription { get; set; } = string.Empty;

        [JsonPropertyName("pages")]
        public IEnumerable<ComicPage> Pages { get; set; } = Enumerable.Empty<ComicPage>();
    }
}
