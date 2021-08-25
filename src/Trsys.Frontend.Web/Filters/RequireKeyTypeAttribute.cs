using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Filters
{
    public class RequireKeyTypeAttribute : ActionFilterAttribute
    {
        public RequireKeyTypeAttribute(string keyType)
        {
            KeyType = keyType;
        }

        public string KeyType { get; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var keyType = (string)context.HttpContext.Request.Headers["X-Ea-Type"];
            if (keyType != KeyType)
            {
                context.Result = new BadRequestObjectResult("InvalidRequest");
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
