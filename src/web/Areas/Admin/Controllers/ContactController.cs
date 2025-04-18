using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Contact;
using X.PagedList.EF;
namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ContactController> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Admin/Contact
    public async Task<IActionResult> Index(string? searchTerm = null, ContactStatus? status = null, int page = 1, int pageSize = 20)
    {
        ViewData["Title"] = "Quản lý Liên hệ - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Contact>().AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(c => c.FullName.ToLower().Contains(lowerSearchTerm)
                                  || c.Email.ToLower().Contains(lowerSearchTerm)
                                  || (c.Phone != null && c.Phone.Contains(lowerSearchTerm))
                                  || c.Subject.ToLower().Contains(lowerSearchTerm)
                                  || c.Message.ToLower().Contains(lowerSearchTerm));
        }
        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        var contactsPaged = await query
            .OrderByDescending(c => c.Status == ContactStatus.New)
            .ThenByDescending(c => c.CreatedAt)
            .ProjectTo<ContactListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        ViewBag.Statuses = Enum.GetValues(typeof(ContactStatus))
                               .Cast<ContactStatus>()
                               .Select(s => new SelectListItem
                               {
                                   Value = s.ToString(),
                                   Text = s.GetDisplayName()
                               }).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedStatus = status;

        return View(contactsPaged);
    }

    // GET: Admin/Contact/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var contact = await _context.Set<Contact>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            _logger.LogWarning("Details GET: Contact with ID {ContactId} not found.", id);
            return NotFound();
        }

        bool markedAsRead = false;
        if (contact.Status == ContactStatus.New)
        {
            var trackedContact = await _context.Set<Contact>().FindAsync(id);
            if (trackedContact != null)
            {
                trackedContact.Status = ContactStatus.Read;
                try
                {
                    await _context.SaveChangesAsync();
                    markedAsRead = true;
                    _logger.LogInformation("Contact ID {ContactId} marked as Read.", id);
                }
                catch (Exception ex) { _logger.LogWarning(ex, "Failed to mark contact {ContactId} as Read.", id); }
            }
        }

        var viewModel = _mapper.Map<ContactDetailViewModel>(contact);
        if (markedAsRead) viewModel.Status = ContactStatus.Read;

        viewModel.StatusList = GetStatusSelectList(viewModel.Status);

        ViewData["Title"] = "Chi tiết Liên hệ - Hệ thống quản trị";
        ViewData["PageTitle"] = "Chi tiết Liên hệ";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))), ("Chi tiết", "") };

        return View(viewModel);
    }

    // POST: Admin/Contact/UpdateDetails/5 (Partial update: Status and AdminNotes)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDetails(int id, ContactDetailViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("UpdateDetails POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("UpdateDetails POST: Model state invalid for Contact ID {ContactId}.", id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return BadRequest(ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()));

            TempData["error"] = "Dữ liệu cập nhật không hợp lệ.";
            viewModel.StatusList = GetStatusSelectList(viewModel.Status);

            var originalContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (originalContact != null)
            {
                viewModel.FullName = originalContact.FullName;
                viewModel.Email = originalContact.Email;
                viewModel.Phone = originalContact.Phone;
                viewModel.Subject = originalContact.Subject;
                viewModel.Message = originalContact.Message;
                viewModel.CompanyName = originalContact.CompanyName;
                viewModel.ProjectDetails = originalContact.ProjectDetails;
                viewModel.CreatedAt = originalContact.CreatedAt;
                viewModel.UpdatedAt = originalContact.UpdatedAt;
                viewModel.IpAddress = originalContact.IpAddress;
                viewModel.UserAgent = originalContact.UserAgent;
            }
            else
            {
                return NotFound();
            }


            ViewData["Title"] = "Chi tiết Liên hệ - Hệ thống quản trị";
            ViewData["PageTitle"] = "Chi tiết Liên hệ";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Liên hệ", Url.Action(nameof(Index))), ("Chi tiết", "") };
            return View("Details", viewModel);
        }

        var contact = await _context.Set<Contact>().FindAsync(id);
        if (contact == null)
        {
            _logger.LogWarning("UpdateDetails POST: Contact with ID {ContactId} not found for update.", id);
            return NotFound();
        }

        bool changed = false;
        if (contact.Status != viewModel.Status)
        {
            contact.Status = viewModel.Status;
            changed = true;
            _logger.LogInformation("Contact ID {ContactId} status changed to {Status} by {User}.", id, viewModel.Status, User.Identity?.Name ?? "Unknown");
        }
        if (contact.AdminNotes != viewModel.AdminNotes)
        {
            contact.AdminNotes = viewModel.AdminNotes;
            changed = true;
            _logger.LogInformation("Contact ID {ContactId} admin notes updated by {User}.", id, User.Identity?.Name ?? "Unknown");
        }


        if (!changed)
        {
            TempData["info"] = "Không có thay đổi nào về trạng thái hoặc ghi chú.";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        try
        {
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true, message = "Cập nhật thành công." });

            TempData["success"] = "Cập nhật trạng thái/ghi chú thành công!";
            return RedirectToAction(nameof(Details), new { id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating contact details for ID: {ContactId}", id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return StatusCode(500, new { message = "Lỗi khi lưu thay đổi." });

            ModelState.AddModelError("", "Không thể lưu thay đổi.");
            TempData["error"] = "Không thể lưu thay đổi.";
            viewModel.StatusList = GetStatusSelectList(viewModel.Status);

            var originalContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (originalContact != null)
            {
                viewModel.FullName = originalContact.FullName;
                viewModel.Email = originalContact.Email;
                viewModel.Phone = originalContact.Phone;
                viewModel.Subject = originalContact.Subject;
                viewModel.Message = originalContact.Message;
                viewModel.CompanyName = originalContact.CompanyName;
                viewModel.ProjectDetails = originalContact.ProjectDetails;
                viewModel.CreatedAt = originalContact.CreatedAt;
                viewModel.UpdatedAt = originalContact.UpdatedAt;
                viewModel.IpAddress = originalContact.IpAddress;
                viewModel.UserAgent = originalContact.UserAgent;
            }
            ViewData["Title"] = "Chi tiết Liên hệ - Hệ thống quản trị";
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
            _logger.LogWarning("Delete POST: Contact with ID {ContactId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy liên hệ." });
        }

        try
        {
            string subject = contact.Subject;
            _context.Remove(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Contact ID {ContactId} ('{Subject}') deleted successfully by {User}.", id, subject, User.Identity?.Name ?? "Unknown");
            return Json(new { success = true, message = "Xóa liên hệ thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Contact ID {ContactId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa liên hệ." });
        }
    }


    // --- Helper Methods ---
    private SelectList GetStatusSelectList(ContactStatus? selectedStatus = null)
    {
        var statuses = Enum.GetValues(typeof(ContactStatus))
                           .Cast<ContactStatus>()
                           .Select(s => new SelectListItem
                           {
                               Value = s.ToString(),
                               Text = s.GetDisplayName()
                           });
        return new SelectList(statuses, "Value", "Text", selectedStatus?.ToString());
    }
}
