using WebComicProvider.Domain.Repositories.Interfaces;
using WebComicProvider.Interfaces;
using WebComicProvider.Models.Comics;

namespace WebComicProviderApi.Managers.Comics
{
    public sealed class ComicsManager : IComicsManager
    {
        private readonly IComicRepository comicRepository;


        public ComicsManager(IComicRepository comicRepository)
        {
            this.comicRepository = comicRepository;
        }


        public async Task<IEnumerable<SimpleComicResponse>> GetAllComics()
        {
            var comics = await comicRepository.GetAllComics();
            return comics.Select(c => new SimpleComicResponse
            {
                ComicId = c.ID,
                ComicName = c.Name,
                Issues = c.Issues,
                Synopsis = c.Description,
                TotalPages = c.Pages
            });
        }


        

    }
}