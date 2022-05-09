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

        public ComicsController(IComicsManager comicsManager)
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

    }

}
