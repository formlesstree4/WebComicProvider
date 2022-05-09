using Microsoft.AspNetCore.Mvc;

namespace WebComicProviderApi.Controllers
{
    public abstract class WebComicProviderApiControllerBase : ControllerBase
    {
        public IActionResult NotImplemented(string message = "") => Problem(statusCode: StatusCodes.Status501NotImplemented, detail: message);

        public IActionResult NotFound(string message = "") => Problem(statusCode: StatusCodes.Status404NotFound, detail: message);

        public IActionResult InternalServerError(string message = "") => Problem(statusCode: StatusCodes.Status500InternalServerError, detail: message);
    }
}
