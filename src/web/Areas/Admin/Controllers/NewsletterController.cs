using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public partial class NewsletterController : Controller
{
    private readonly INewsletterService _newsletterService;
    private readonly IMapper _mapper;
    private readonly ILogger<NewsletterController> _logger;
    private readonly IValidator<NewsletterViewModel> _newsletterViewModelValidator;

    public NewsletterController(
        INewsletterService newsletterService,
        IMapper mapper,
        ILogger<NewsletterController> logger,
        IValidator<NewsletterViewModel> newsletterViewModelValidator)
    {
        _newsletterService = newsletterService ?? throw new ArgumentNullException(nameof(newsletterService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _newsletterViewModelValidator = newsletterViewModelValidator ?? throw new ArgumentNullException(nameof(newsletterViewModelValidator));
    }

    // GET: Admin/Newsletter
    public async Task<IActionResult> Index(NewsletterFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new NewsletterFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<NewsletterListItemViewModel> newslettersPaged = await _newsletterService.GetPagedNewslettersAsync(filter, pageNumber, currentPageSize);

        filter.StatusOptions = GetStatusSelectList(filter.IsActive);

        NewsletterIndexViewModel viewModel = new()
        {
            Newsletters = newslettersPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Newsletter/Create
    public IActionResult Create()
    {
        NewsletterViewModel viewModel = new()
        {
            IsActive = true
        };
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsletterViewModel viewModel)
    {
        var result = await _newsletterViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers["User-Agent"].ToString();


        var createResult = await _newsletterService.CreateNewsletterAsync(viewModel, ipAddress, userAgent);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm đăng ký email thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Email", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Email), error);
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm đăng ký email '{viewModel.Email}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Newsletter/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        NewsletterViewModel? viewModel = await _newsletterService.GetNewsletterByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Newsletter not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy đăng ký để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // POST: Admin/Newsletter/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsletterViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _newsletterViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _newsletterService.UpdateNewsletterAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật đăng ký thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Email", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Email), error);
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật đăng ký email '{viewModel.Email}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Newsletter/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _newsletterService.DeleteNewsletterAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa đăng ký thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa đăng ký.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class NewsletterController
{
    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang hoạt động", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không hoạt động", Selected = selectedValue == false }
        };
    }
}
