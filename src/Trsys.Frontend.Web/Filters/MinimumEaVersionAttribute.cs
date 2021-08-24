using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Trsys.Frontend.Web.Filters
{
    public class MinimumEaVersionAttribute : ActionFilterAttribute
    {
        public MinimumEaVersionAttribute(string version)
        {
            Version = version;
        }

        public string Version { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var version = (string)context.HttpContext.Request.Headers["X-Ea-Version"];
            if (string.IsNullOrEmpty(version))
            {
                context.Result = new BadRequestObjectResult("InvalidVersion");
                return;
            }
            if (version.CompareTo(Version) < 0)
            {
                context.Result = new BadRequestObjectResult("InvalidVersion");
                return;
            }
        }
    }
}
