using WebComicProvider.Domain;
using WebComicProvider.Domain.Comics;
using WebComicProvider.Domain.Repositories.Interfaces;
using WebComicProvider.Interfaces;
using WebComicProvider.Models.Comics;

namespace WebComicProviderApi.Managers.Comics
{
    public sealed class ComicsManager : IComicsManager
    {
        private readonly IComicRepository comicRepository;
        private readonly IUserManager userManager;
        private readonly IImageManager imageManager;

        public ComicsManager(IComicRepository comicRepository, IUserManager userManager, IImageManager imageManager)
        {
            this.comicRepository = comicRepository;
            this.userManager = userManager;
            this.imageManager = imageManager;
        }


        public async Task<IEnumerable<SimpleComicResponse>> GetAllComics()
        {
            var comics = await comicRepository.GetAllComics();
            return comics.Select(c => new SimpleComicResponse
            {
                ComicId = c.ID,
                ComicName = c.Name,
                Cover = c.Cover,
                Issues = c.Issues,
                Synopsis = c.Description,
                TotalPages = c.Pages
            });
        }

        public async Task<SimpleComicResponse> GetSimpleComicDetails(int comicId)
        {
            var comicDetails = await comicRepository.GetComic(comicId);
            return new SimpleComicResponse
            {
                ComicId = comicDetails.ID,
                ComicName = comicDetails.Name,
                Cover = comicDetails.Cover,
                CreatedOn = comicDetails.CreatedOn,
                Synopsis = comicDetails.Description,
                TotalPages = comicDetails.Pages,
                UpdatedOn = comicDetails.UpdatedOn,
                Issues = comicDetails.Issues
            };
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
                    Cover = comicDetails.Item1.Cover,
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
        


        public async Task<SimpleComicResponse> CreateComic(string name, string description, int createdByUserId, int status, Stream cover, string coverName)
        {
            var imageData = await imageManager.SaveImage(cover, Path.GetExtension(coverName));
            var comicModel = new ComicModel(default, name, description, createdByUserId, DateTimeOffset.Now, DateTimeOffset.Now, status, imageData.ImageId, default, default);
            var comicId = await comicRepository.SaveComic(comicModel);
            return await GetSimpleComicDetails(comicId);
        }

        public async Task<SimpleComicResponse> UpdateComic(int comicId, string name, string description, int createdByUserId, int status, Stream cover, string coverName)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteComic(int comicId, int requestingUserId)
        {
            throw new NotImplementedException();
        }


        public async Task CreateComicIssue(int comicId, int requestingUserId, ComicIssue issue)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComicIssue(int comicId, int requestingUserId, ComicIssue issue)
        {
            throw new NotImplementedException();
        }
        
        public async Task DeleteComicIssue(int comicId, int issueId, int requestingUserId)
        {
            throw new NotImplementedException();
        }


        public async Task<ComicPage> CreateComicPage(int comicId, int issueId, ComicPage page)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateComicPage(int comicId, int issueId, ComicPage page)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteComicPage(int pageId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Statuses>> GetStatuses()
        {
            var statuses = await comicRepository.GetComicStatuses();
            return statuses.Select(s => (Statuses)s.StatusId);
        }

    }

}