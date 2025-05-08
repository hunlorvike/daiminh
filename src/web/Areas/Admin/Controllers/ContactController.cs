using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Validators.Contact;
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
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu cập nhật không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await new ContactViewModelValidator().ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await RefillViewModelFromDbAsync(id, viewModel);
            return View("Details", viewModel);
        }

        var contact = await _context.Set<Contact>().FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy liên hệ để cập nhật.", ToastType.Error)
            );
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
            try
            {
                await _context.SaveChangesAsync();
                TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                    new ToastData("Thành công", "Cập nhật trạng thái và ghi chú thành công.", ToastType.Success)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật liên hệ: {Id}", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật liên hệ.");
                await RefillViewModelFromDbAsync(id, viewModel);
                return View("Details", viewModel);
            }
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thông báo", "Không có thay đổi nào được lưu.", ToastType.Info)
            );
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST: Admin/Contact/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Set<Contact>().FindAsync(id);
        if (contact == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy liên hệ.", ToastType.Error)
            );
            return Json(new { success = false, message = "Không tìm thấy liên hệ." });
        }

        try
        {
            string subject = contact.Subject;
            _context.Remove(contact);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Xóa liên hệ '{subject}' thành công.", ToastType.Success)
            );
            return Json(new { success = true, message = $"Xóa liên hệ '{subject}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa liên hệ ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa liên hệ.", ToastType.Error)
            );
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa liên hệ." });
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

    private async Task RefillViewModelFromDbAsync(int id, ContactViewModel viewModel)
    {
        var contact = await _context.Set<Contact>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (contact != null)
        {
            viewModel.FullName = contact.FullName;
            viewModel.Email = contact.Email;
            viewModel.Phone = contact.Phone;
            viewModel.Subject = contact.Subject;
            viewModel.Message = contact.Message;
            viewModel.CreatedAt = contact.CreatedAt;
            viewModel.IpAddress = contact.IpAddress;
            viewModel.UserAgent = contact.UserAgent;
        }

        viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);
    }
}