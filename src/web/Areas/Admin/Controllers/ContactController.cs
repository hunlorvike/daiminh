using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ContactViewModel> _validator;

    public ContactController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ContactViewModel> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    // GET: Admin/Contact
    public async Task<IActionResult> Index(ContactStatus? status = null, string? searchTerm = null)
    {
        ViewData["PageTitle"] = "Quản lý liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "")
        };

        IQueryable<Contact> query = _context.Set<Contact>().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.FullName.Contains(searchTerm) ||
                c.Email.Contains(searchTerm) ||
                c.Subject.Contains(searchTerm) ||
                c.Message.Contains(searchTerm));
        }

        var contacts = await query
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<ContactListItemViewModel>>(contacts);

        ViewBag.StatusFilter = status;
        ViewBag.SearchTerm = searchTerm;
        ViewBag.NewCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.New);
        ViewBag.InProgressCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.InProgress);
        ViewBag.CompletedCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.Completed);
        ViewBag.SpamCount = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.Spam);

        return View(viewModels);
    }

    // GET: Admin/Contact/Details/5
    public async Task<IActionResult> Details(int id)
    {
        ViewData["PageTitle"] = "Chi tiết liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "/Admin/Contact"),
            ("Chi tiết", "")
        };

        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ContactViewModel>(contact);

        return View(viewModel);
    }

    // GET: Admin/Contact/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["PageTitle"] = "Cập nhật liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Liên hệ", "/Admin/Contact"),
            ("Cập nhật", "")
        };

        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ContactViewModel>(contact);

        return View(viewModel);
    }

    // POST: Admin/Contact/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContactViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);

            ViewData["PageTitle"] = "Cập nhật liên hệ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
            {
                ("Liên hệ", "/Admin/Contact"),
                ("Cập nhật", "")
            };

            return View(viewModel);
        }

        try
        {
            var contact = await _context.Set<Contact>().FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            // Cập nhật các trường có thể chỉnh sửa
            contact.Status = viewModel.Status;
            contact.AdminNotes = viewModel.AdminNotes;
            contact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật liên hệ thành công";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ContactExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: Admin/Contact/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return Json(new { success = false, message = "Không tìm thấy liên hệ" });
        }

        _context.Set<Contact>().Remove(contact);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Xóa liên hệ thành công" });
    }

    // POST: Admin/Contact/ChangeStatus/5
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, ContactStatus status)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);

        if (contact == null)
        {
            return Json(new { success = false, message = "Không tìm thấy liên hệ" });
        }

        contact.Status = status;
        contact.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        string statusText = status switch
        {
            ContactStatus.New => "Mới",
            ContactStatus.InProgress => "Đang xử lý",
            ContactStatus.Completed => "Đã xử lý",
            ContactStatus.Spam => "Spam",
            _ => status.ToString()
        };

        return Json(new { success = true, message = $"Đã chuyển trạng thái thành {statusText}" });
    }

    private async Task<bool> ContactExists(int id)
    {
        return await _context.Set<Contact>().AnyAsync(e => e.Id == id);
    }
}
