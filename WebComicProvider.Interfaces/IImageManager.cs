using WebComicProvider.Models.Comics.Images;

namespace WebComicProvider.Interfaces
{

    /// <summary>
    /// Abstraction layer for saving and loading images
    /// </summary>
    public interface IImageManager
    {

        /// <summary>
        /// Saves an image to a remote destination and returns a key that can be used to uniquely identify the image
        /// </summary>
        /// <param name="imageStream"><see cref="Stream"/></param>
        /// <param name="format">The format, or extension, to save as</param>
        /// <returns><see cref="ImageMetaData"/></returns>
        Task<ImageMetaData> SaveImage(Stream imageStream, string format);

        /// <summary>
        /// Retrieves the metadata about a given image
        /// </summary>
        /// <param name="imageId">The unique ID for the image</param>
        /// <returns><see cref="ImageMetaData"/></returns>
        Task<ImageMetaData> GetImageMetaData(string imageId);

        /// <summary>
        /// Retrieves a <see cref="Stream"/> that allows for read access to the image
        /// </summary>
        /// <param name="imageId">The unique ID for the image</param>
        /// <returns><see cref="Stream"/></returns>
        Task<Stream> LoadImage(string imageId);

        /// <summary>
        /// Deletes a given image from the server
        /// </summary>
        /// <param name="imageId">The unique ID for the image</param>
        /// <returns>A promise to delete the image</returns>
        Task DeleteImage(string imageId);

    }
}
