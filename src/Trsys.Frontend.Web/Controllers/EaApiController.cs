using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trsys.CopyTrading.Abstractions;
using Trsys.CopyTrading.Application;
using Trsys.Frontend.Web.Filters;

namespace Trsys.Frontend.Web.Controllers
{
    [EaEndpoint]
    [MinimumEaVersion("20210609")]
    [ApiController]
    public class EaApiController : ControllerBase
    {
        private readonly IEaService service;

        public EaApiController(IEaService service)
        {
            this.service = service;
        }

        [Route("api/keys")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostKey([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            await service.AddSecretKeyAsync(key, keyType);
            return Ok();
        }

        [Route("api/token")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostToken([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType)
        {
            try
            {
                var session = await service.GenerateTokenAsync(key, keyType);
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
            var result = await service.InvalidateSessionAsync(token, key, keyType);
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
            var result = await service.ValidateSessionAsync(token, key, "Subscriber");
            if (!result)
            {
                return BadRequest("InvalidToken");
            }

            var orderText = await service.GetOrderTextAsync(key);
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

            HttpContext.Response.Headers["ETag"] = $"\"{orderText.Hash}\"";
            return Ok(orderText.Text);
        }

        [Route("api/orders")]
        [HttpPost]
        [Consumes("text/plain")]
        [RequireToken]
        [RequireKeyType("Publisher")]
        public async Task<IActionResult> PostOrders([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Secret-Token")] string token, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            var result = await service.ValidateSessionAsync(token, key, "Publisher");
            if (!result)
            {
                return BadRequest("InvalidToken");
            }

            try
            {
                await service.PublishOrderTextAsync(key, text);
                return Ok();
            }
            catch (OrderTextFormatException)
            {
                return BadRequest("InvalidOrderText");
            }
        }

        [Route("api/logs")]
        [HttpPost]
        [Consumes("text/plain")]
        public async Task<IActionResult> PostLogs([FromHeader(Name = "X-Ea-Id")] string key, [FromHeader(Name = "X-Ea-Type")] string keyType, [FromHeader(Name = "X-Secret-Token")] string token, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string text)
        {
            await service.ReceiveLogAsync(key, keyType, token, text);
            return Ok();
        }
    }
}
