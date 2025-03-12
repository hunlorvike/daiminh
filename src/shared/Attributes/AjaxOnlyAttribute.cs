using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using shared.Extensions;

namespace shared.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AjaxOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.HttpContext.Request.IsAjaxRequest()) filterContext.Result = new NotFoundResult();

        base.OnActionExecuting(filterContext);
    }
}