using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebComicProvider.Interfaces;

namespace WebComicProviderApi.Controllers
{

    [ApiController, AllowAnonymous]
    [Route("api/[controller]")]
    public class ComicsController : WebComicProviderApiControllerBase
    {
        private readonly IComicsManager comicsManager;

        public ComicsController(IComicsManager comicsManager, IImageManager imageManager)
        {
            this.comicsManager = comicsManager;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await comicsManager.GetAllComics());
        }

        [HttpGet("{comicId:int}")]
        public async Task<IActionResult> GetComicDetails(int comicId)
        {
            return Ok(await comicsManager.GetComicDetails(comicId));
        }

        [HttpGet("{comicId:int}/cover")]
        public async Task<IActionResult> GetComicCover(int comicId)
        {
            var details = await comicsManager.GetComicDetails(comicId);
            
            //return await Task.FromResult(NotImplemented());
        }

        [HttpGet("{comicId:int}/{issueId:int}/{pageId:int}/image")]
        public async Task<IActionResult> GetPageImage(int comicId, int issueId, int pageId)
        {
            return await Task.FromResult(NotImplemented());
        }




        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateComic()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest();
            }

            var formCollection = await Request.ReadFormAsync();

            foreach (var file in formCollection.Files)
            {
                
            }


            return Ok();
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
