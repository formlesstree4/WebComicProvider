using WebComicProvider.Domain;
using WebComicProvider.Models.Comics;

namespace WebComicProvider.Interfaces
{
    public interface IComicsManager
    {
        Task<IEnumerable<SimpleComicResponse>> GetAllComics();
        Task<SimpleComicResponse> GetSimpleComicDetails(int comicId);
        Task<ComplexComicResponse> GetComicDetails(int comicId);
        Task<SimpleComicResponse> CreateComic(string name, string description, int createdByUserId, int status, Stream cover);
        Task<SimpleComicResponse> UpdateComic(int comicId, string name, string description, int createdByUserId, int status, Stream cover);
        Task DeleteComic(int comicId, int requestingUserId);
        Task CreateComicIssue(int comicId, int requestingUserId, ComicIssue issue);
        Task UpdateComicIssue(int comicId, int requestingUserId, ComicIssue issue);
        Task DeleteComicIssue(int comicId, int issueId, int requestingUserId);
        Task<ComicPage> CreateComicPage(int comicId, int issueId, ComicPage page);
        Task UpdateComicPage(int comicId, int issueId, ComicPage page);
        Task DeleteComicPage(int pageId, int userId);
        Task<IEnumerable<Statuses>> GetStatuses();
    }
}
