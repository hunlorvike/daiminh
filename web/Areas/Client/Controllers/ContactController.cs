using core.Entities;
using infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using web.Areas.Admin.Models.Contact;

namespace web.Areas.Client.Controllers;

[Area("Client")]
[Route("/lien-he")]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SubmitContact(ContactViewModel model)
    {

        if (model == null)
        {
            TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        try
        {
            var contact = new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Message = model.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cảm ơn bạn đã liên hệ! Chúng tôi sẽ phản hồi sớm nhất.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Đã có lỗi xảy ra, vui lòng thử lại!";
            Console.WriteLine($"Lỗi khi lưu liên hệ: {ex.Message}");
        }
        return RedirectToAction("Index");
    }
}