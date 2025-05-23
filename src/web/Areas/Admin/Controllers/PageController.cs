using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminAccess")]
public partial class PageController : Controller
{
    private readonly IPageService _pageService;
    private readonly ILogger<PageController> _logger;
    private readonly IValidator<PageViewModel> _pageViewModelValidator;

    public PageController(
        IPageService pageService,
        ILogger<PageController> logger,
        IValidator<PageViewModel> pageViewModelValidator)
    {
        _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _pageViewModelValidator = pageViewModelValidator ?? throw new ArgumentNullException(nameof(pageViewModelValidator));
    }

    // GET: Admin/Page
    public async Task<IActionResult> Index(PageFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new PageFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<PageListItemViewModel> pagesPaged = await _pageService.GetPagedPagesAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.Status);

        PageIndexViewModel viewModel = new()
        {
            Pages = pagesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Page/Create
    public IActionResult Create()
    {
        PageViewModel viewModel = new()
        {
            Status = PublishStatus.Draft,
            PublishedAt = null
        };

        return View(viewModel);
    }

    // POST: Admin/Page/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PageViewModel viewModel)
    {
        var result = await _pageViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var createResult = await _pageService.CreatePageAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm trang thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            {
                ModelState.AddModelError(string.Empty, createResult.Message);
            }

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm trang '{viewModel.Title}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Page/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        PageViewModel? viewModel = await _pageService.GetPageByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Trang không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy trang để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }

    // POST: Admin/Page/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PageViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _pageViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _pageService.UpdatePageAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật trang thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            {
                ModelState.AddModelError(string.Empty, updateResult.Message);
            }


            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật trang '{viewModel.Title}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }


    // POST: Admin/Page/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _pageService.DeletePageAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa trang thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa trang.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class PageController
{
    private List<SelectListItem> GetStatusSelectList(PublishStatus? selectedValue)
    {
        var statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>();

        var selectList = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue }
        };

        foreach (var status in statuses)
        {
            selectList.Add(new SelectListItem
            {
                Value = status.ToString(),
                Text = status.GetDisplayName(),
                Selected = selectedValue.HasValue && selectedValue.Value == status
            });
        }

        return selectList;
    }
}