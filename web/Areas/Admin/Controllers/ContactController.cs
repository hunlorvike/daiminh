using core.Services;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactController(ContactService contactService) : Controller
{
    private readonly ContactService _contactService = contactService;

    public IActionResult Index()
    {
        return View();
    }
}