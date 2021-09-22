using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Diagnostics;
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
            Activity.Current.AddEvent(new ActivityEvent("PostKeyStart"));
            await service.AddSecretKeyAsync(key, keyType);
            Activity.Current.AddEvent(new ActivityEvent("PostKeySuccess"));
            return Ok();
        }

        [Route("api/keys/delete")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostDeleteKey([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            Activity.Current.AddEvent(new ActivityEvent("DeleteKeyStart"));
            await service.RemvoeSecretKeyAsync(key, keyType);
            Activity.Current.AddEvent(new ActivityEvent("DeleteKeySuccess"));
            return Ok();
        }

        [Route("api/token")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostToken([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            Activity.Current.AddEvent(new ActivityEvent("PostTokenStart"));
            try
            {
                var session = await service.GenerateSessionTokenAsync(key, keyType);
                if (session is null)
                {
                    Activity.Current.AddEvent(new ActivityEvent("PostTokenSessionNotFound"));
                    return BadRequest("InvalidSecretKey");
                }
                cache.UpdateValidEaSessionTokenValidity(session.Token);
                Activity.Current.AddEvent(new ActivityEvent("PostTokenSuccess"));
                return Ok(session.Token);
            }
            catch (EaSessionAlreadyExistsException)
            {
                Activity.Current.AddEvent(new ActivityEvent("PostTokenKeyInUse"));
                return BadRequest("SecretKeyInUse");
            }
        }

        [Route("api/token/{token}/release")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostTokenRelease([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType, string token)
        {
            Activity.Current.AddEvent(new ActivityEvent("DeleteTokenStart"));
            try
            {
                await service.DiscardSessionTokenAsync(token, key, keyType);
                cache.RemoveValidEaSessionToken(token);
                Activity.Current.AddEvent(new ActivityEvent("DeleteTokenSuccess"));
                return Ok();
            }
            catch (EaSessionTokenNotFoundException)
            {
                Activity.Current.AddEvent(new ActivityEvent("DeleteTokenUnknownToken"));
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                Activity.Current.AddEvent(new ActivityEvent("DeleteTokenInvalidToken"));
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
            Activity.Current.AddEvent(new ActivityEvent("GetOrdersStart"));
            try
            {
                if (cache.IsValidEaSessionToken(token) == ValidateEaSessionTokenCacheResult.NOT_IN_CACHE)
                {
                    Activity.Current.AddEvent(new ActivityEvent("GetOrdersSessionCacheNotHit"));
                    await service.ValidateSessionTokenAsync(token, key, "Subscriber");
                    cache.UpdateValidEaSessionTokenValidity(token);
                }
                var orderText = cache.GetOrderTextCache();
                if (orderText == null)
                {
                    Activity.Current.AddEvent(new ActivityEvent("GetOrdersOrderTextCacheNotHit"));
                    orderText = await service.GetCurrentOrderTextAsync();
                    cache.UpdateOrderTextCache(orderText);
                }
                var etags = HttpContext.Request.Headers["If-None-Match"];
                if (etags.Any())
                {
                    foreach (var etag in etags)
                    {
                        if (etag == $"\"{orderText.Hash}\"")
                        {
                            Activity.Current.AddEvent(new ActivityEvent("GetOrdersNotModified"));
                            return StatusCode(304);
                        }
                    }
                }
                await service.SubscribeOrderTextAsync(key, orderText.Text);
                HttpContext.Response.Headers["ETag"] = $"\"{orderText.Hash}\"";
                Activity.Current.AddEvent(new ActivityEvent("GetOrdersSuccess"));
                return Ok(orderText.Text);
            }
            catch (EaSessionTokenNotFoundException)
            {
                Activity.Current.AddEvent(new ActivityEvent("GetOrdersUnknownToken"));
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                Activity.Current.AddEvent(new ActivityEvent("GetOrdersInvalidToken"));
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
            Activity.Current.AddEvent(new ActivityEvent("PostOrdersStart"));
            try
            {
                if (cache.IsValidEaSessionToken(token) == ValidateEaSessionTokenCacheResult.NOT_IN_CACHE)
                {
                    Activity.Current.AddEvent(new ActivityEvent("PostOrdersSessionCacheNotHit"));
                    await service.ValidateSessionTokenAsync(token, key, "Publisher");
                    cache.UpdateValidEaSessionTokenValidity(token);
                }
                await service.PublishOrderTextAsync(key, text);
                cache.RemoveOrderTextCache();
                Activity.Current.AddEvent(new ActivityEvent("PostOrdersSuccess"));
                return Ok();
            }
            catch (EaSessionTokenNotFoundException)
            {
                Activity.Current.AddEvent(new ActivityEvent("PostOrdersUnknownToken"));
                return BadRequest("InvalidToken");
            }
            catch (EaSessionTokenInvalidException)
            {
                Activity.Current.AddEvent(new ActivityEvent("PostOrdersInvalidToken"));
                return BadRequest("InvalidToken");
            }
            catch (OrderTextFormatException)
            {
                Activity.Current.AddEvent(new ActivityEvent("PostOrdersInvalidOrderTextFormat"));
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
