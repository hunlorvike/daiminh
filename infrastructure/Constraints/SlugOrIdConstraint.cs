using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace infrastructure.Constraints;

public class SlugOrIdConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        var value = values[routeKey]?.ToString();
        if (string.IsNullOrEmpty(value)) return false;

        if (int.TryParse(value, out _))
            return true;

        return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-z0-9\-]+$");
    }
}