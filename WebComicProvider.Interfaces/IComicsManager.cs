using WebComicProvider.Models.Comics;

namespace WebComicProvider.Interfaces
{
    public interface IComicsManager
    {
        Task<IEnumerable<SimpleComicResponse>> GetAllComics();
        Task<ComplexComicResponse> GetComicDetails(int comicId);
    }
}
