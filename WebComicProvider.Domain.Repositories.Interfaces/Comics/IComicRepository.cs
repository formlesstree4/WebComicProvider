using WebComicProvider.Domain.Comics;

namespace WebComicProvider.Domain.Repositories.Interfaces
{
    public interface IComicRepository
    {
        Task<(ComicModel, Dictionary<IssueModel, IEnumerable<PageModel>>)> Get(int comicId);
        Task<IEnumerable<ComicModel>> GetAllComics();
    }
}