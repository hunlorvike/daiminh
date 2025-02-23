using core.Services;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Models.Contact;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/lien-he")]
//public class ContactController(ContactService contactService) : Controller
//{
//    private readonly ContactService _contactService = contactService;

//    [HttpGet]
//    [Route("")]
//    public IActionResult Index()
//    {
//        return View();
//    }
//}

public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var contacts = await _context.Contacts.ToListAsync();
        var contactViewModels = contacts.Select(c => new ContactViewModel
        {
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Message = c.Message,
            CreatedAt = c.CreatedAt
        }).ToList();
        return View(contactViewModels);
    }

    [HttpPost]
    [Route("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thành công!";
        }
        return RedirectToAction("Index");
    }
}