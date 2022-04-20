using Microsoft.AspNetCore.Mvc;

namespace WebComicProviderApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ComicsController : BaseController
    {
        




        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(NotImplemented());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComicDetails(int comicId)
        {
            return await Task.FromResult(NotImplemented());
        }

    }

}
