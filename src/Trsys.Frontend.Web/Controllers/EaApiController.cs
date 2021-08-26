using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
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
            try
            {
                var session = await EaService.Instance.GenerateTokenAsync(key, keyType);
                if (session is null)
                {
                    return BadRequest("InvalidSecretKey");
                }
                return Ok(session.Token);
            }
            catch (EaSessionAlreadyExistsException)
            {
                return BadRequest("SecretKeyInUse");
            }
        }

        [Route("api/token/{token}/release")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostTokenRelease([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType, string token)
        {
            var result = await EaService.Instance.InvalidateSessionAsync(token, key, keyType);
            if (!result)
            {
                return BadRequest("InvalidToken");
            }
            return Ok();
        }

        [Route("api/orders")]
        [HttpGet]
        [Produces("text/plain")]
        [RequireToken]
        [RequireKeyType("Subscriber")]
        public async Task<IActionResult> GetOrders([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Secret-Token")] string token)
        {
            var result = await EaService.Instance.ValidateSessionAsync(token, key, "Subscriber");
            if (!result)
            {
                return BadRequest("InvalidToken");
            }
            HttpContext.Response.Headers["ETag"] = $"\"ETAG\"";
            return Ok("");
        }

        [Route("api/orders")]
        [HttpPost]
        [Consumes("text/plain")]
        [RequireToken]
        [RequireKeyType("Publisher")]
        public async Task<IActionResult> PostOrders([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Secret-Token")] string token, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            var result = await EaService.Instance.ValidateSessionAsync(token, key, "Publisher");
            if (!result)
            {
                return BadRequest("InvalidToken");
            }
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
