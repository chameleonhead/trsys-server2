using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.Frontend.Web.Caching;
using Trsys.Frontend.Web.Filters;

namespace Trsys.Frontend.Web.Controllers
{
    [EaEndpoint]
    [MinimumEaVersion("20210609")]
    [ApiController]
    public class EaApiController : ControllerBase
    {
        private readonly IEaService service;
        private readonly CopyTradingCache cache;

        public EaApiController(IEaService service, CopyTradingCache cache)
        {
            this.service = service;
            this.cache = cache;
        }

        [Route("api/keys")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostKey([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            await service.AddSecretKeyAsync(key, keyType);
            return Ok();
        }

        [Route("api/keys/delete")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostDeleteKey([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            await service.RemvoeSecretKeyAsync(key, keyType);
            return Ok();
        }

        [Route("api/token")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostToken([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            try
            {
                var session = await service.GenerateSessionTokenAsync(key, keyType);
                if (session is null)
                {
                    return BadRequest("InvalidSecretKey");
                }
                cache.UpdateValidEaSessionTokenValidity(session.Token);
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
            try
            {
                await service.DiscardSessionTokenAsync(token, key, keyType);
                cache.RemoveValidEaSessionToken(token);
                return Ok();
            }
            catch (EaSessionTokenNotFoundException)
            {
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                return BadRequest("InvalidToken");
            }
        }

        [Route("api/orders")]
        [HttpGet]
        [Produces("text/plain")]
        [RequireToken]
        [RequireKeyType("Subscriber")]
        public async Task<IActionResult> GetOrders([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Secret-Token")] string token)
        {
            try
            {
                if (cache.IsValidEaSessionToken(token) == ValidateEaSessionTokenCacheResult.NOT_IN_CACHE)
                {
                    await service.ValidateSessionTokenAsync(token, key, "Subscriber");
                    cache.UpdateValidEaSessionTokenValidity(token);
                }
                var orderText = cache.GetOrderTextHash(key);
                if (orderText == null)
                {
                    orderText = await service.GetOrderTextAsync(key);
                    cache.UpdateOrderTextCache(key, orderText);
                }
                var etags = HttpContext.Request.Headers["If-None-Match"];
                if (etags.Any())
                {
                    foreach (var etag in etags)
                    {
                        if (etag == $"\"{orderText.Hash}\"")
                        {
                            return StatusCode(304);
                        }
                    }
                }
                orderText = await service.GetOrderTextAsync(key);
                HttpContext.Response.Headers["ETag"] = $"\"{orderText.Hash}\"";
                return Ok(orderText.Text);
            }
            catch (EaSessionTokenNotFoundException)
            {
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                return BadRequest("InvalidToken");
            }
        }

        [Route("api/orders")]
        [HttpPost]
        [Consumes("text/plain")]
        [RequireToken]
        [RequireKeyType("Publisher")]
        public async Task<IActionResult> PostOrders([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Secret-Token")] string token, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            try
            {
                if (cache.IsValidEaSessionToken(token) == ValidateEaSessionTokenCacheResult.NOT_IN_CACHE)
                {
                    await service.ValidateSessionTokenAsync(token, key, "Publisher");
                    cache.UpdateValidEaSessionTokenValidity(token);
                }
                await service.PublishOrderTextAsync(key, text);
                return Ok();
            }
            catch (EaSessionTokenNotFoundException)
            {
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                return BadRequest("InvalidToken");
            }
            catch (OrderTextFormatException)
            {
                return BadRequest("InvalidOrderText");
            }
        }

        [Route("api/logs")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostLogs([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType, [FromHeader(Name = "X-Ea-Version")] string version, [FromHeader(Name = "X-Secret-Token")] string token, [FromHeader(Name = "X-Ea-Timestamp")] string timestamp, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            var now = DateTimeOffset.Now;
            if (string.IsNullOrEmpty(timestamp) || !long.TryParse(timestamp, out var eaTimestamp))
            {
                await service.ReceiveLogAsync(now, key, keyType, version, token, text);
            }
            else
            {
                await service.ReceiveLogAsync(now, eaTimestamp, key, keyType, version, token, text);
            }
            return Ok();
        }
    }
}
