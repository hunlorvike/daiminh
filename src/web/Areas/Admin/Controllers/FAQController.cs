using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = "AdminAccess")]
public class FAQController : Controller
{
    private readonly IFAQService _faqService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<FAQController> _logger;
    private readonly IValidator<FAQViewModel> _faqViewModelValidator;


    public FAQController(
       IFAQService faqService,
       ICategoryService categoryService,
       ILogger<FAQController> logger,
       IValidator<FAQViewModel> faqViewModelValidator)
    {
        _faqService = faqService ?? throw new ArgumentNullException(nameof(faqService));
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _faqViewModelValidator = faqViewModelValidator ?? throw new ArgumentNullException(nameof(faqViewModelValidator));
    }

    // GET: Admin/FAQ
    public async Task<IActionResult> Index(FAQFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new FAQFilterViewModel();

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<FAQListItemViewModel> faqsPaged = await _faqService.GetPagedFAQsAsync(filter, pageNumber, currentPageSize);

        filter.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, filter.CategoryId);

        FAQIndexViewModel viewModel = new()
        {
            FAQs = faqsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/FAQ/Create
    public async Task<IActionResult> Create()
    {
        FAQViewModel viewModel = new()
        {
            IsActive = true,
            OrderIndex = 0,
        };

        viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ);

        return View(viewModel);
    }

    // POST: Admin/FAQ/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQViewModel viewModel)
    {
        var result = await _faqViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, viewModel.CategoryId);
            return View(viewModel);
        }

        var createResult = await _faqService.CreateFAQAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm FAQ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Danh mục cha", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm FAQ '{viewModel.Question}'.", ToastType.Error)
            );

            viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, viewModel.CategoryId);
            return View(viewModel);
        }
    }

    // GET: Admin/FAQ/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        FAQViewModel? viewModel = await _faqService.GetFAQByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("FAQ not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy FAQ để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, viewModel.CategoryId);

        return View(viewModel);
    }

    // POST: Admin/FAQ/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FAQViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _faqViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, viewModel.CategoryId);
            return View(viewModel);
        }

        var updateResult = await _faqService.UpdateFAQAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật FAQ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Danh mục cha", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật FAQ '{viewModel.Question}'.", ToastType.Error)
            );

            viewModel.Categories = await _categoryService.GetParentCategorySelectListAsync(CategoryType.FAQ, viewModel.CategoryId);
            return View(viewModel);
        }
    }

    // POST: Admin/FAQ/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _faqService.DeleteFAQAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa FAQ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa FAQ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}