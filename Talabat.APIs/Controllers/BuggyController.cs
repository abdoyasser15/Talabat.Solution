using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            string str = null;
            return Ok(str.ToString()); // هيرمي Exception
        }

        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }
    }
}
