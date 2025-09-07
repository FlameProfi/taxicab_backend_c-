using Microsoft.AspNetCore.Mvc;

namespace course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(T result, string notFoundMessage = "Запись не найдена")
        {
            if (result == null)
                return NotFound(new { message = notFoundMessage });

            return Ok(result);
        }

        protected IActionResult HandleCreated<T>(T result, string actionName = "Get")
        {
            return CreatedAtAction(actionName, result);
        }

        protected IActionResult HandleError(string message, int statusCode = 500)
        {
            return StatusCode(statusCode, new { message = message });
        }
    }
}