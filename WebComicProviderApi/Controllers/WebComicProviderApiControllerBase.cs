using Microsoft.AspNetCore.Mvc;

namespace WebComicProviderApi.Controllers
{
    public abstract class WebComicProviderApiControllerBase : ControllerBase
    {
        protected IActionResult NotImplemented(string message = "") => Problem(statusCode: StatusCodes.Status501NotImplemented, detail: message);

        protected IActionResult NotFound(string message = "") => Problem(statusCode: StatusCodes.Status404NotFound, detail: message);

        protected IActionResult InternalServerError(string message = "") => Problem(statusCode: StatusCodes.Status500InternalServerError, detail: message);
    }
}
