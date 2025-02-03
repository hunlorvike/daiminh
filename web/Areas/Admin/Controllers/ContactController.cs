using core.Common.Enums;
using core.Entities;
using core.Services;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactController : Controller
{
    private readonly ContactService _contactService;

    public ContactController(ContactService contactService)
    {
        _contactService = contactService;
    }

    public IActionResult Index()
    {
        return View();
    }
}