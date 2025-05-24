using System.Text.Json;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactController> _logger;
    private readonly IValidator<ContactViewModel> _contactViewModelValidator;


    public ContactController(
        IContactService contactService,
        IMapper mapper,
        ILogger<ContactController> logger,
        IValidator<ContactViewModel> contactViewModelValidator)
    {
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contactViewModelValidator = contactViewModelValidator ?? throw new ArgumentNullException(nameof(contactViewModelValidator));
    }

    // GET: Admin/Contact
    [Authorize(Policy = PermissionConstants.ContactView)]
    public async Task<IActionResult> Index(ContactFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new ContactFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<ContactListItemViewModel> contactsPaged = await _contactService.GetPagedContactsAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusOptionsSelectList(filter.Status);

        ContactIndexViewModel viewModel = new()
        {
            Contacts = contactsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Contact/Details/5
    [Authorize(Policy = PermissionConstants.ContactView)]
    public async Task<IActionResult> Details(int id)
    {
        ContactViewModel? viewModel = await _contactService.GetContactByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Contact not found for details. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy liên hệ.", ToastType.Error)
            );
            return NotFound();
        }

        viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);

        return View(viewModel);
    }

    // POST: Admin/Contact/UpdateDetails/5
    [Authorize(Policy = PermissionConstants.ContactEdit)]
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

        var result = await _contactViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await _contactService.RefillContactViewModelFromDbAsync(viewModel);
            viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);
            return View("Details", viewModel);
        }

        var updateResult = await _contactService.UpdateContactDetailsAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật thành công.", ToastType.Success)
            );
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? "Không thể cập nhật liên hệ.", ToastType.Error)
            );

            await _contactService.RefillContactViewModelFromDbAsync(viewModel);
            viewModel.StatusOptions = GetStatusOptionsSelectList(viewModel.Status);
            return View("Details", viewModel);
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST: Admin/Contact/Delete/5
    [Authorize(Policy = PermissionConstants.ContactDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _contactService.DeleteContactAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa liên hệ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa liên hệ.", ToastType.Error)
            );
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
