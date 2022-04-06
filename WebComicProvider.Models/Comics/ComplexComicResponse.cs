using System.Text.Json.Serialization;

namespace WebComicProvider.Models.Comics
{
    public sealed class ComplexComicResponse
    {

        [JsonPropertyName("metadata")]
        public SimpleComicResponse Metadata { get; set; } = new();

        [JsonPropertyName("issues")]
        public IEnumerable<ComicIssue> Issues { get; set; } = Enumerable.Empty<ComicIssue>();

        /// <summary>
        /// Gets or sets the last Issue number that the current user was on for this comic
        /// </summary>
        [JsonPropertyName("lastIssue")]
        public int LastIssue { get; set; } = 1;

        /// <summary>
        /// Gets or sets the last Page number that the current user was on for this comic
        /// </summary>
        [JsonPropertyName("lastPage")]
        public int LastPage { get; set; } = 1;

    }
}
