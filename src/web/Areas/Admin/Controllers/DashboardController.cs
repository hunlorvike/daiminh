using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;


namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public class DashboardController : Controller
{
    [Authorize(Policy = "Dashboard.View")]
    public IActionResult Index()
    {

        return View();
    }
}
