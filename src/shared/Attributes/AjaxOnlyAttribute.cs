using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using shared.Extensions;

namespace shared.Attributes;

/// <summary>
/// An action filter attribute that restricts access to an action method to only AJAX requests.
/// If a non-AJAX request is made, a 404 Not Found result is returned.
/// </summary>
/// <remarks>
/// This attribute can be applied to either a class (to affect all actions in a controller)
/// or to individual action methods. It relies on the <see cref="HttpRequestExtensions.IsAjaxRequest"/>
/// extension method to determine if the request is an AJAX request.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AjaxOnlyAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Called before the action method is invoked. Checks if the request is an AJAX request.
    /// </summary>
    /// <param name="filterContext">The context for the action execution.</param>
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // Check if the request is an AJAX request using the extension method.
        if (!filterContext.HttpContext.Request.IsAjaxRequest())
        {
            // If not an AJAX request, return a 404 Not Found result.
            filterContext.Result = new NotFoundResult();
        }

        // Continue with the action execution.
        base.OnActionExecuting(filterContext);
    }
}