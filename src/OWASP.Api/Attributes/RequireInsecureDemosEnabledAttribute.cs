using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OWASP.Api.Attributes;

public class RequireInsecureDemosEnabledAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var config = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
        var enabled = config?.GetValue<bool>("Features:EnableInsecureDemos") ?? false;
        if (!enabled)
        {
            context.Result = new ForbidResult();
        }
    }
}
