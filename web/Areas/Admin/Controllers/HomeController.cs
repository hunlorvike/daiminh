using core.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin}, {RoleConstants.Manager}",
    AuthenticationSchemes = CookiesConstants.AdminCookieSchema)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}