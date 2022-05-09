using WebComicProvider.Domain.Repositories.Interfaces;
using WebComicProvider.Interfaces;
using WebComicProvider.Models.Comics;

namespace WebComicProviderApi.Managers.Comics
{
    public sealed class ComicsManager : IComicsManager
    {
        private readonly IComicRepository comicRepository;
        private readonly IUserManager userManager;

        public ComicsManager(IComicRepository comicRepository, IUserManager userManager)
        {
            this.comicRepository = comicRepository;
            this.userManager = userManager;
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

        public async Task<ComplexComicResponse> GetComicDetails(int comicId)
        {
            var comicDetails = await comicRepository.Get(comicId);
            var creator = await userManager.Get(comicDetails.Item1.CreatedBy);
            if (creator is null) throw new InvalidOperationException("Unable to load Author");

            return new ComplexComicResponse
            {
                Metadata = new SimpleComicResponse
                {
                    Author = creator.Username,
                    ComicId = comicDetails.Item1.ID,
                    ComicName = comicDetails.Item1.Name,
                    CreatedOn = comicDetails.Item1.CreatedOn,
                    Synopsis = comicDetails.Item1.Description,
                    TotalPages = comicDetails.Item1.Pages,
                    UpdatedOn = comicDetails.Item1.UpdatedOn,
                    Issues = comicDetails.Item1.Issues
                },
                Issues = comicDetails.Item2
                    .Where(issue => issue.Key.ReleaseDate <= DateTimeOffset.Now)
                    .OrderBy(issue => issue.Key.Number)
                    .Select(issue => new ComicIssue
                    {
                        IssueDescription = issue.Key.Synopsis,
                        IssueId = issue.Key.ID,
                        IssueName = issue.Key.Name,
                        IssueNumber = issue.Key.Number,
                        Pages = issue.Value
                        .Where(page => page.ReleaseDate <= DateTimeOffset.Now)
                        .OrderBy(page => page.Number)
                        .Select(page => new ComicPage
                        {
                            Commentary = page.Commentary,
                            Location = page.Location,
                            PageId = page.ID,
                            PageNumber = page.Number,
                            PublishDate = page.ReleaseDate,
                            Title = page.Title,
                            ToolTip = page.ToolTip
                        })
                    })
            };
        }
        

    }
}