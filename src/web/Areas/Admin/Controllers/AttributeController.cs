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
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public class AttributeController : Controller
{
    private readonly IAttributeService _attributeService;
    private readonly ILogger<AttributeController> _logger;
    private readonly IValidator<AttributeViewModel> _attributeViewModelValidator;

    public AttributeController(
        IAttributeService attributeService,
        ILogger<AttributeController> logger,
        IValidator<AttributeViewModel> attributeViewModelValidator)
    {
        _attributeService = attributeService ?? throw new ArgumentNullException(nameof(attributeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _attributeViewModelValidator = attributeViewModelValidator ?? throw new ArgumentNullException(nameof(attributeViewModelValidator));
    }

    // GET: Admin/Attribute
    public async Task<IActionResult> Index(AttributeFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new AttributeFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<AttributeListItemViewModel> attributesPaged = await _attributeService.GetPagedAttributesAsync(filter, pageNumber, currentPageSize);

        AttributeIndexViewModel viewModel = new()
        {
            Attributes = attributesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Attribute/Create
    public IActionResult Create()
    {
        AttributeViewModel viewModel = new();
        return View(viewModel);
    }

    // POST: Admin/Attribute/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeViewModel viewModel)
    {
        var result = await _attributeViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            return View(viewModel);
        }

        var createResult = await _attributeService.CreateAttributeAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm thuộc tính thành công.", ToastType.Success)
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm thuộc tính '{viewModel.Name}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // GET: Admin/Attribute/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        AttributeViewModel? viewModel = await _attributeService.GetAttributeByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Attribute not found for editing. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy thuộc tính để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        return View(viewModel);
    }

    // POST: Admin/Attribute/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttributeViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _attributeViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(viewModel);
        }

        var updateResult = await _attributeService.UpdateAttributeAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật thuộc tính thành công.", ToastType.Success)
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật thuộc tính '{viewModel.Name}'.", ToastType.Error)
            );
            return View(viewModel);
        }
    }

    // POST: Admin/Attribute/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _attributeService.DeleteAttributeAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa thuộc tính thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa thuộc tính.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}