using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Controllers.Shared;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public partial class AuthController(
    IMapper mapper,
    IServiceProvider serviceProvider,
    IConfiguration configuration)
    : DaiminhController(mapper, serviceProvider, configuration);

public partial class AuthController
{
    [Route("/dang-nhap")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("/dang-ky")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("/quen-mat-khau")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
}