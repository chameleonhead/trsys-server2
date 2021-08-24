using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Trsys.Frontend.Controllers
{
    [ApiController]
    public class EaApiController : ControllerBase
    {
        [Route("api/token")]
        [HttpPost]
        [Consumes("text/plain")]
        public IActionResult PostToken([FromBody] string key)
        {
            return Ok("SECRETKEY");
        }

        [Route("api/token/{token}/release")]
        [HttpPost]
        [Consumes("text/plain")]
        public IActionResult PostTokenRelease(string token)
        {
            return Ok();
        }

        [Route("api/orders")]
        [HttpGet]
        [Produces("text/plain")]
        // [RequireToken("Subscriber")]
        public IActionResult GetOrders()
        {
            HttpContext.Response.Headers["ETag"] = $"\"ETAG\"";
            return Ok("");
        }

        [Route("api/orders")]
        [HttpPost]
        [Consumes("text/plain")]
        // [RequireToken("Publisher")]
        public IActionResult PostOrder([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            return Ok();
        }

        [Route("api/logs")]
        [HttpPost]
        [Consumes("text/plain")]
        public IActionResult PostLogs([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            return Ok();
        }
    }
}
