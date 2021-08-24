using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Trsys.Frontend.Web.Filters;
using Trsys.Frontend.Web.Services;

namespace Trsys.Frontend.Web.Controllers
{
    [EaEndpoint]
    [MinimumEaVersion("20210609")]
    [ApiController]
    public class EaApiController : ControllerBase
    {
        [Route("api/token")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostToken([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            var session = await EaService.Instance.GenerateTokenAsync(key, keyType);
            if (session is null)
            {
                return BadRequest("InvalidToken");
            }
            return Ok(session.Token);
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
