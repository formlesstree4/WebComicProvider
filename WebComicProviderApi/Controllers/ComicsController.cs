using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebComicProvider.Domain;
using WebComicProvider.Interfaces;

namespace WebComicProviderApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ComicsController : WebComicProviderApiControllerBase
    {
        private readonly IComicsManager comicsManager;
        private readonly IImageManager imageManager;

        public ComicsController(IComicsManager comicsManager, IImageManager imageManager)
        {
            this.comicsManager = comicsManager;
            this.imageManager = imageManager;
        }



        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(await comicsManager.GetAllComics());
        }

        [HttpGet("{comicId:int}"), AllowAnonymous]
        public async Task<IActionResult> GetComicDetails(int comicId)
        {
            return Ok(await comicsManager.GetComicDetails(comicId));
        }

        [HttpGet("{comicId:int}/cover"), AllowAnonymous]
        public async Task<IActionResult> GetComicCover(int comicId)
        {
            var details = await comicsManager.GetComicDetails(comicId);
            var imageMetadata = await imageManager.GetImageMetaData(details.Metadata.Cover);
            return File(imageManager.OpenImage(details.Metadata.Cover), imageMetadata.MimeType);
        }

        [HttpGet("{comicId:int}/{issueId:int}/{pageId:int}/image"), AllowAnonymous]
        public async Task<IActionResult> GetPageImage(int comicId, int issueId, int pageId)
        {
            var details = await comicsManager.GetComicDetails(comicId);
            var issue = details.Issues.FirstOrDefault(i => i.IssueId == issueId);
            if (issue is null) return BadRequest();

            var page = issue.Pages.FirstOrDefault(i => i.PageId == pageId);
            if (page is null || page.Location is null) return BadRequest();

            var imageMetadata = await imageManager.GetImageMetaData(page.Location);
            return File(imageManager.OpenImage(page.Location), imageMetadata.MimeType);
        }

        [HttpGet("statuses"), AllowAnonymous]
        public async Task<IActionResult> GetComicStatuses()
        {
            var statuses = await comicsManager.GetStatuses();
            return Ok(statuses.Select(s => new { Id = (int)s, Status = s.GetDescription() }));
        }



        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateComic()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest();
            }

            var formCollection = await Request.ReadFormAsync();
            var name = formCollection["name"];
            var description = formCollection["description"];
            var status = formCollection["status"];
            var userId = User.GetUserId();

            if (userId is null) return BadRequest();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(status))
            {
                return BadRequest();
            }
            
            if (formCollection.Files.Count == 0)
            {
                return BadRequest();
            }

            using var imageStream = formCollection.Files[0].OpenReadStream();
            var savedComic = await comicsManager.CreateComic(name, description, userId.Value, (int)Statuses.ComicActive, imageStream, formCollection.Files[0].FileName);
            return Ok(savedComic);
        }

        [HttpPut("update"), Authorize]
        public IActionResult UpdateComic()
        {
            return NotImplemented();
        }

        [HttpDelete("{comicId:int}"), Authorize]
        public IActionResult DeleteComic(int comicId)
        {
            return NotImplemented();
        }

    }

}
