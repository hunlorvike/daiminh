using AutoMapper;
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
public class AttributeValueController : Controller
{
    private readonly IAttributeValueService _attributeValueService;
    private readonly IAttributeService _attributeService;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeValueController> _logger;
    private readonly IValidator<AttributeValueViewModel> _attributeValueViewModelValidator;

    public AttributeValueController(
        IAttributeValueService attributeValueService,
        IAttributeService attributeService,
        IMapper mapper,
        ILogger<AttributeValueController> logger,
        IValidator<AttributeValueViewModel> attributeValueViewModelValidator)
    {
        _attributeValueService = attributeValueService ?? throw new ArgumentNullException(nameof(attributeValueService));
        _attributeService = attributeService ?? throw new ArgumentNullException(nameof(attributeService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _attributeValueViewModelValidator = attributeValueViewModelValidator ?? throw new ArgumentNullException(nameof(attributeValueViewModelValidator));
    }

    // GET: Admin/AttributeValue
    public async Task<IActionResult> Index(AttributeValueFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new AttributeValueFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<AttributeValueListItemViewModel> valuesPaged = await _attributeValueService.GetPagedAttributeValuesAsync(filter, pageNumber, currentPageSize);

        filter.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(filter.AttributeId);

        AttributeValueIndexViewModel viewModel = new()
        {
            AttributeValues = valuesPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/AttributeValue/Create
    public async Task<IActionResult> Create(int? attributeId = null)
    {
        AttributeValueViewModel viewModel = new()
        {
            AttributeId = attributeId ?? 0
        };
        viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);
        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AttributeValueViewModel viewModel)
    {
        var result = await _attributeValueViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);
            return View(viewModel);
        }

        var createResult = await _attributeValueService.CreateAttributeValueAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm giá trị thuộc tính thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { AttributeId = viewModel.AttributeId });
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else if (error.Contains("Thuộc tính cha", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.AttributeId), error);
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm giá trị '{viewModel.Value}'.", ToastType.Error)
            );
            viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);
            return View(viewModel);
        }
    }

    // GET: Admin/AttributeValue/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        AttributeValueViewModel? viewModel = await _attributeValueService.GetAttributeValueByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("AttributeValue not found for editing. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy giá trị thuộc tính để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);

        return View(viewModel);
    }

    // POST: Admin/AttributeValue/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AttributeValueViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var result = await _attributeValueViewModelValidator.ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);
            return View(viewModel);
        }

        var updateResult = await _attributeValueService.UpdateAttributeValueAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật giá trị thuộc tính thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { AttributeId = viewModel.AttributeId });
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Slug", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), error);
                }
                else if (error.Contains("Thuộc tính cha", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.AttributeId), error);
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật giá trị '{viewModel.Value}'.", ToastType.Error)
            );
            viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.AttributeId);
            return View(viewModel);
        }
    }

    // POST: Admin/AttributeValue/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _attributeValueService.DeleteAttributeValueAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa giá trị thuộc tính thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa giá trị thuộc tính.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}
