using Microsoft.AspNetCore.Mvc;
using WebComicProvider.Interfaces;

namespace WebComicProviderApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ComicsController : WebComicProviderApiControllerBase
    {
        private readonly IComicsManager comicsManager;

        public ComicsController(IComicsManager comicsManager)
        {
            this.comicsManager = comicsManager;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await comicsManager.GetAllComics());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComicDetails(int comicId)
        {
            return Ok(await comicsManager.GetComicDetails(comicId));
        }

    }

}
