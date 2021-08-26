using Microsoft.AspNetCore.Mvc;
using Trsys.Frontend.Web.Models.Session;

namespace Trsys.Frontend.Web.Controllers
{
    [Route("api/session")]
    [ApiController]
    public class SessionApiController : ControllerBase
    {
        [HttpPost]
        [Produces("application/json", Type = typeof(PostSessionResponse))]
        public IActionResult PostToken(PostSessionRequest request)
        {
            return Ok(new PostSessionResponse()
            {
                Token = "TOKEN",
                RefreshToken = "REFRESH_TOKEN",
            });
        }
    }
}
