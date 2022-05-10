using System.Text.Json.Serialization;

namespace WebComicProvider.Models.Comics.Images
{

    /// <summary>
    /// Metadata class about a given image that's been saved
    /// </summary>
    public sealed class ImageMetaData
    {

        /// <summary>
        /// Gets or sets the unique identifier for the image
        /// </summary>
        [JsonPropertyName("imageId")]
        public string ImageId { get; set; } = "";


        /// <summary>
        /// Gets or sets the calculated SHA hash for the image
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; } = "";

        /// <summary>
        /// Gets or sets the MIME type information for the given image
        /// </summary>
        [JsonPropertyName("mime")]
        public string MimeType { get; set; } = "";

    }
}
