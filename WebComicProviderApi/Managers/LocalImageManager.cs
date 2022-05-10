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
            _baseFolder = Path.Combine(environment.ContentRootPath, configuration["Storage:Local:Subfolder"]);
        }



        public async Task DeleteImage(string imageId)
        {
            await Extensions.DeleteAsync(Path.Combine(_baseFolder, imageId));
        }

        public async Task<ImageMetaData> GetImageMetaData(string imageId)
        {
            var imageData = await LoadImage(imageId);
            return new ImageMetaData
            {
                Hash = (await Extensions.CalculateImageHashBytes(imageData)).BytesToString(),
                ImageId = imageId,
                MimeType = MimeTypes.GetMimeType(imageId)
            };
        }

        public Task<Stream> LoadImage(string imageId)
        {
            return Task.FromResult(new FileStream(Path.Combine(_baseFolder, imageId), FileMode.Open, FileAccess.Read) as Stream);
        }

        public async Task<ImageMetaData> SaveImage(Stream imageStream, string format)
        {
            var imageId = $"{Guid.NewGuid()}.{format}";
            using var writer = new FileStream(Path.Combine(_baseFolder, imageId), FileMode.CreateNew, FileAccess.ReadWrite);
            await imageStream.CopyToAsync(writer);
            return new ImageMetaData
            {
                Hash = (await Extensions.CalculateImageHashBytes(imageStream)).BytesToString(),
                ImageId = imageId,
                MimeType = MimeTypes.GetMimeType(imageId)
            };
        }
    }
}
