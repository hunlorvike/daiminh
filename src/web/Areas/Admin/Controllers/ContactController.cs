using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Contact;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Roles for managing contacts?
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ContactDetailViewModel> _validator; // Validator for edit
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ContactDetailViewModel> validator,
         ILogger<ContactController> logger)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    // GET: Admin/Contact
    public async Task<IActionResult> Index(string? searchTerm = null, ContactStatus? status = null)
    {
        ViewData["PageTitle"] = "Quản lý Liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", "") };

        var query = _context.Set<Contact>().AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.FullName.Contains(searchTerm)
                                  || c.Email.Contains(searchTerm)
                                  || c.Phone.Contains(searchTerm)
                                  || c.Subject.Contains(searchTerm)
                                  || c.Message.Contains(searchTerm));
        }
        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        var contacts = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<ContactListItemViewModel>>(contacts);

        // Load filter dropdown
        ViewBag.Statuses = Enum.GetValues(typeof(ContactStatus))
                               .Cast<ContactStatus>()
                               .Select(s => new SelectListItem
                               {
                                   Value = s.ToString(),
                                   Text = GetStatusDisplayName(s)
                               }).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedStatus = status;

        return View(viewModels);
    }

    // GET: Admin/Contact/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);
        if (contact == null) return NotFound();

        // Mark as Read when viewed? (Optional)
        if (contact.Status == ContactStatus.New)
        {
            contact.Status = ContactStatus.Read;
            try { await _context.SaveChangesAsync(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to mark contact {ContactId} as Read.", id); } // Log but continue
        }

        var viewModel = _mapper.Map<ContactDetailViewModel>(contact);
        viewModel.StatusList = GetStatusSelectList();

        ViewData["PageTitle"] = "Chi tiết Liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))), ("Chi tiết", "") };

        return View(viewModel);
    }

    // POST: Admin/Contact/UpdateDetails/5 (Partial update: Status and AdminNotes)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDetails(int id, ContactDetailViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        var validationResult = await _validator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            // If submitting via AJAX, return bad request with errors
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return BadRequest(validationResult.ToDictionary());

            // If full page post, return view with errors
            validationResult.AddToModelState(ModelState);
            TempData["ErrorMessage"] = "Dữ liệu cập nhật không hợp lệ.";
            viewModel.StatusList = GetStatusSelectList(); // Reload dropdown
            // Need to reload non-editable fields if returning view
            var originalContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (originalContact != null) _mapper.Map(originalContact, viewModel); // Map original data back

            ViewData["PageTitle"] = "Chi tiết Liên hệ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))), ("Chi tiết", "") };
            return View("Details", viewModel);
        }

        var contact = await _context.Set<Contact>().FindAsync(id);
        if (contact == null) return NotFound();

        // Apply changes for Status and AdminNotes only
        contact.Status = viewModel.Status;
        contact.AdminNotes = viewModel.AdminNotes;
        // UpdatedAt and UpdatedBy should be handled by SaveChanges interceptor/override

        try
        {
            await _context.SaveChangesAsync();

            // AJAX response
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, message = "Cập nhật thành công." });

            TempData["SuccessMessage"] = "Cập nhật trạng thái/ghi chú thành công!";
            return RedirectToAction(nameof(Details), new { id = id }); // Redirect back to details
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contact details for ID: {ContactId}", id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return StatusCode(500, new { message = "Lỗi khi lưu thay đổi." });

            ModelState.AddModelError("", "Không thể lưu thay đổi.");
            TempData["ErrorMessage"] = "Không thể lưu thay đổi.";
            viewModel.StatusList = GetStatusSelectList();
            var originalContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (originalContact != null) _mapper.Map(originalContact, viewModel);
            ViewData["PageTitle"] = "Chi tiết Liên hệ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))), ("Chi tiết", "") };
            return View("Details", viewModel);
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
            return Json(new { success = false, message = "Không tìm thấy liên hệ." });
        }

        _context.Remove(contact);
        try
        {
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa liên hệ thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Contact ID {ContactId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa liên hệ." });
        }
    }


    // --- Helper Methods ---
    private SelectList GetStatusSelectList()
    {
        return new SelectList(Enum.GetValues(typeof(ContactStatus))
                              .Cast<ContactStatus>()
                              .Select(s => new { Value = s, Text = GetStatusDisplayName(s) }),
                              "Value", "Text");
    }

    private string GetStatusDisplayName(ContactStatus status)
    {
        return status switch
        {
            ContactStatus.New => "Mới",
            ContactStatus.Read => "Đã đọc",
            ContactStatus.Replied => "Đã trả lời",
            ContactStatus.InProgress => "Đang xử lý",
            ContactStatus.Resolved => "Đã giải quyết",
            ContactStatus.Spam => "Spam/Rác",
            ContactStatus.Archived => "Lưu trữ",
            _ => status.ToString()
        };
    }
}