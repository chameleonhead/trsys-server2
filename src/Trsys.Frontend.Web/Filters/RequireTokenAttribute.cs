using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Trsys.Frontend.Web.Filters
{
    public class RequireTokenAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = (string)context.HttpContext.Request.Headers["X-Secret-Token"];
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new BadRequestObjectResult("InvalidToken");
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
