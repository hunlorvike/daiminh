using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Contact;
using X.PagedList;
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
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Contact
    public async Task<IActionResult> Index(ContactFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new ContactFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IQueryable<Contact> query = _context.Set<Contact>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.FullName.ToLower().Contains(lowerSearchTerm)
                                  || c.Email.ToLower().Contains(lowerSearchTerm)
                                  || c.Subject.ToLower().Contains(lowerSearchTerm)
                                  || c.Message.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(c => c.Status == filter.Status.Value);
        }

        query = query.OrderByDescending(c => c.CreatedAt);

        IPagedList<ContactListItemViewModel> contactsPaged = await query
            .ProjectTo<ContactListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusOptionsSelectList(filter.Status);

        ContactIndexViewModel viewModel = new()
        {
            Contacts = contactsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Contact/Details/5
    public async Task<IActionResult> Details(int id)
    {
        Contact? contact = await _context.Set<Contact>()
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(c => c.Id == id);

        if (contact == null)
        {
            return NotFound();
        }

        ContactViewModel viewModel = _mapper.Map<ContactViewModel>(contact);
        viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);

        return View(viewModel);
    }

    // POST: Admin/Contact/UpdateDetails/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDetails(int id, ContactViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return BadRequest("ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);
            var originalContact = await _context.Set<Contact>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (originalContact != null)
            {
                viewModel.FullName = originalContact.FullName;
                viewModel.Email = originalContact.Email;
                viewModel.Phone = originalContact.Phone;
                viewModel.Subject = originalContact.Subject;
                viewModel.Message = originalContact.Message;
                viewModel.CreatedAt = originalContact.CreatedAt;
                viewModel.IpAddress = originalContact.IpAddress;
                viewModel.UserAgent = originalContact.UserAgent;
            }
            else
            {
                return NotFound();
            }
            return View("Details", viewModel);
        }

        Contact? contact = await _context.Set<Contact>().FirstOrDefaultAsync(c => c.Id == id);

        if (contact == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy liên hệ để cập nhật.";
            return RedirectToAction(nameof(Index));
        }

        bool changed = false;
        if (contact.Status != viewModel.Status)
        {
            contact.Status = viewModel.Status;
            changed = true;
        }
        if (contact.AdminNotes != viewModel.AdminNotes)
        {
            contact.AdminNotes = viewModel.AdminNotes;
            changed = true;
        }

        if (changed)
        {
            _context.Entry(contact).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật trạng thái và ghi chú thành công!";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật liên hệ.");
                viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);
                var originalContact = await _context.Set<Contact>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (originalContact != null)
                {
                    viewModel.FullName = originalContact.FullName;
                    viewModel.Email = originalContact.Email;
                    viewModel.Phone = originalContact.Phone;
                    viewModel.Subject = originalContact.Subject;
                    viewModel.Message = originalContact.Message;
                    viewModel.CreatedAt = originalContact.CreatedAt;
                    viewModel.IpAddress = originalContact.IpAddress;
                    viewModel.UserAgent = originalContact.UserAgent;
                }
                return View("Details", viewModel);
            }
        }
        else
        {
            TempData["InfoMessage"] = "Không có thay đổi nào được lưu.";
        }

        return RedirectToAction(nameof(Details), new { id = contact.Id });
    }


    // POST: Admin/Contact/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Contact? contact = await _context.Set<Contact>().FindAsync(id);
        if (contact == null)
        {
            return Json(new { success = false, message = "Không tìm thấy liên hệ." });
        }

        try
        {
            string subject = contact.Subject;
            _context.Remove(contact);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Xóa liên hệ '{subject}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Đã xảy ra lỗi không mong muốn khi xóa liên hệ.";
            return RedirectToAction(nameof(Index));
        }
    }

    private List<SelectListItem> GetStatusOptionsSelectList(ContactStatus? selectedValue)
    {
        var items = Enum.GetValues(typeof(ContactStatus))
            .Cast<ContactStatus>()
            .Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.GetDisplayName(),
                Selected = selectedValue.HasValue && e == selectedValue.Value
            })
            .OrderBy(e => e.Text)
            .ToList();

        return items;
    }
}