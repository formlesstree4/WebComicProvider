using WebComicProvider.Domain.Comics;

namespace WebComicProvider.Domain.Repositories.Interfaces
{
    public interface IComicRepository
    {
        /// <summary>
        /// Gets the specific details of a particular comic
        /// </summary>
        /// <param name="comicId">The ID of the comic</param>
        /// <returns>A tuple that contains the full comic model and a key-value pair of issues and pages</returns>
        Task<(ComicModel, Dictionary<IssueModel, IEnumerable<PageModel>>)> Get(int comicId);

        /// <summary>
        /// Gets only the metadata of a particular comic
        /// </summary>
        /// <param name="comicId">The ID of the comic</param>
        /// <returns><see cref="ComicModel"/></returns>
        Task<ComicModel> GetComic(int comicId);

        /// <summary>
        /// Gets all the comics available
        /// </summary>
        /// <returns>A collection of comic book entities</returns>
        Task<IEnumerable<ComicModel>> GetAllComics();

        /// <summary>
        /// Gets the collection of valid comic statuses
        /// </summary>
        /// <returns>A collection of tuples where the string is the status name and the integer is the status ID</returns>
        Task<IEnumerable<StatusModel>> GetComicStatuses();
    }
}