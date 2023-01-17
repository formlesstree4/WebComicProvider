using WebComicProvider.Interfaces;
using WebComicProvider.Models.Comics.Images;

namespace WebComicProviderApi.Managers
{
    public sealed class LocalImageManager : IImageManager
    {

        private readonly string _baseFolder;


        public LocalImageManager(IWebHostEnvironment environment, IConfiguration configuration)
        {
            var storageMode = configuration["Storage:Mode"];
            if (!storageMode.Equals("Local", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{nameof(LocalImageManager)} was instantiated when it should not be", nameof(configuration));
            }
            _baseFolder = Path.Combine(environment.WebRootPath, configuration["Storage:Local:Subfolder"]);
        }



        public async Task DeleteImage(string imageId)
        {
            await Extensions.DeleteAsync(Path.Combine(_baseFolder, imageId));
        }

        public async Task<ImageMetaData> GetImageMetaData(string imageId)
        {
            var imageData = OpenImage(imageId);
            return await Task.FromResult(new ImageMetaData
            {
                Hash = Extensions.CalculateImageHashBytes(imageData).BytesToString(),
                ImageId = imageId,
                MimeType = MimeTypes.GetMimeType(imageId)
            });
        }

        public Stream OpenImage(string imageId)
        {
            return new FileStream(Path.Combine(_baseFolder, imageId), FileMode.Open, FileAccess.Read) as Stream;
        }

        public async Task<ImageMetaData> SaveImage(Stream imageStream, string extension)
        {
            var imageId = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(_baseFolder, imageId);
            using var writer = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
            await imageStream.CopyToAsync(writer);
            writer.Close();
            using var reader = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new ImageMetaData
            {
                Hash = Extensions.CalculateImageHashBytes(reader).BytesToString(),
                ImageId = imageId,
                MimeType = MimeTypes.GetMimeType(imageId)
            };

        }
    }
}
