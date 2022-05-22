using WebComicProvider.Domain;
using WebComicProvider.Models.Comics;

namespace WebComicProvider.Interfaces
{
    public interface IComicsManager
    {
        /// <summary>
        /// Returns a collection of all available comics
        /// </summary>
        /// <returns>A collection of <see cref="SimpleComicResponse"/></returns>
        Task<IEnumerable<SimpleComicResponse>> GetAllComics();

        /// <summary>
        /// Fetches the details of a specific comic
        /// </summary>
        /// <param name="comicId">The ID of the comic</param>
        /// <returns><see cref="SimpleComicResponse"/></returns>
        Task<SimpleComicResponse> GetSimpleComicDetails(int comicId);

        /// <summary>
        /// Fetches the entire record of the comic
        /// </summary>
        /// <param name="comicId">The ID of the comic</param>
        /// <returns><see cref="ComplexComicResponse"/></returns>
        Task<ComplexComicResponse> GetComicDetails(int comicId);

        /// <summary>
        /// Creates a new comic and returns a <see cref="SimpleComicResponse"/>
        /// </summary>
        /// <param name="name">The name of the comic</param>
        /// <param name="description">The provided description</param>
        /// <param name="createdByUserId">The User who is creating the comic</param>
        /// <param name="status">The current status ID of the comic</param>
        /// <param name="cover">The Image <see cref="Stream"/> that is the cover</param>
        /// <returns><see cref="SimpleComicResponse"/></returns>
        Task<SimpleComicResponse> CreateComic(string name, string description, int createdByUserId, int status, Stream cover);

        /// <summary>
        /// Updates an existing comic with some new details
        /// </summary>
        /// <param name="comicId">The ID of the comic to update</param>
        /// <param name="name">The (potentially new) name of the comic</param>
        /// <param name="description">The (potentially new) description of the comic</param>
        /// <param name="createdByUserId">The User who is updating the comic which must equal the owner</param>
        /// <param name="status">The current status ID of the comic</param>
        /// <param name="cover">The Image <see cref="Stream"/> that is the cover</param>
        /// <returns><see cref="SimpleComicResponse"/></returns>
        Task<SimpleComicResponse> UpdateComic(int comicId, string name, string description, int createdByUserId, int status, Stream cover);

        /// <summary>
        /// Deletes an existing comic
        /// </summary>
        /// <param name="comicId">The ID of the comic to delete</param>
        /// <param name="requestingUserId">The User ID who is requesting a deletion</param>
        /// <returns>A promise to delete the comic if the requesting User ID is valid</returns>
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
