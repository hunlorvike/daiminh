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
public partial class TagController : Controller
{
    private readonly ITagService _tagService;
    private readonly ILogger<TagController> _logger;
    private readonly IValidator<TagViewModel> _tagViewModelValidator;

    public TagController(
        ITagService tagService,
        ILogger<TagController> logger,
        IValidator<TagViewModel> tagViewModelValidator)
    {
        _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tagViewModelValidator = tagViewModelValidator ?? throw new ArgumentNullException(nameof(tagViewModelValidator));
    }


    // GET: Admin/Tag
    public async Task<IActionResult> Index(TagFilterViewModel filter, int page = 1, int pageSize = 10)
    {
        filter ??= new TagFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 10;

        IPagedList<TagListItemViewModel> pagedList = await _tagService.GetPagedTagsAsync(filter, pageNumber, currentPageSize);

        filter.TagTypes = GetTagTypesSelectList(filter.Type);

        TagIndexViewModel viewModel = new()
        {
            Tags = pagedList,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Tag/Create
    public IActionResult Create()
    {
        TagViewModel viewModel = new()
        {
            TagTypes = GetTagTypesSelectList()
        };

        return View(viewModel);
    }

    // POST: Admin/Tag/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TagViewModel viewModel)
    {
        var validationResult = await _tagViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var createResult = await _tagService.CreateTagAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm thẻ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in createResult.Errors)
            {
                if (error.Contains("Tên thẻ", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), error);
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm thẻ '{viewModel.Name}'.", ToastType.Error)
            );
            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }
    }


    // GET: Admin/Tag/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        TagViewModel? viewModel = await _tagService.GetTagByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Tag not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy thẻ để cập nhật.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }

        viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
        return View(viewModel);
    }

    // POST: Admin/Tag/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TagViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _tagViewModelValidator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }

        var updateResult = await _tagService.UpdateTagAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật thẻ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            foreach (var error in updateResult.Errors)
            {
                if (error.Contains("Tên thẻ", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), error);
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật thẻ '{viewModel.Name}'.", ToastType.Error)
            );
            viewModel.TagTypes = GetTagTypesSelectList(viewModel.Type);
            return View(viewModel);
        }
    }


    // POST: Admin/Tag/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _tagService.DeleteTagAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa thẻ thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa thẻ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class TagController
{
    private List<SelectListItem> GetTagTypesSelectList(TagType? selectedType = null)
    {
        List<SelectListItem> tagTypes = Enum.GetValues(typeof(TagType))
            .Cast<TagType>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedType.HasValue && t == selectedType.Value
            })
            .OrderBy(t => t.Text)
            .ToList();

        List<SelectListItem> items =
        [
            new SelectListItem
            {
                Value = "",
                Text = "Tất cả loại thẻ",
                Selected = !selectedType.HasValue
            },
            .. tagTypes,
        ];

        return items;
    }
}