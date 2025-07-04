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
public partial class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;
    private readonly IValidator<ProductViewModel> _productViewModelValidator;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        ITagService tagService,
        IMapper mapper,
        ILogger<ProductController> logger,
        IValidator<ProductViewModel> productViewModelValidator)
    {
        _productService = productService;
        _categoryService = categoryService;
        _tagService = tagService;
        _mapper = mapper;
        _logger = logger;
        _productViewModelValidator = productViewModelValidator;
    }

    [Authorize(Policy = PermissionConstants.ProductView)]
    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1, int pageSize = 15)
    {
        filter ??= new ProductFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 15;

        IPagedList<ProductListItemViewModel> productsPaged = await _productService.GetPagedProductsAsync(filter, pageNumber, currentPageSize);

        filter.CategoryOptions = await _categoryService.GetParentCategorySelectListAsync(CategoryType.Product, filter.CategoryId);
        filter.StatusOptions = GetPublishStatusSelectList(filter.Status);
        filter.ActiveOptions = GetYesNoSelectList(filter.IsActive, "Kích hoạt");
        filter.FeaturedOptions = GetYesNoSelectList(filter.IsFeatured, "Nổi bật");

        ProductIndexViewModel viewModel = new()
        {
            Products = productsPaged,
            Filter = filter
        };
        return View(viewModel);
    }

    [Authorize(Policy = PermissionConstants.ProductCreate)]
    public async Task<IActionResult> Create()
    {
        ProductViewModel viewModel = new()
        {
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.8,
            SitemapChangeFrequency = "weekly"
        };
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    [Authorize(Policy = PermissionConstants.ProductCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        var validationResult = await _productViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var createResult = await _productService.CreateProductAsync(viewModel);
        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Thêm sản phẩm thành công.", ToastType.Success));
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in createResult.Errors)
            ModelState.AddModelError(string.Empty, error);
        if (!createResult.Errors.Any() && !string.IsNullOrEmpty(createResult.Message))
            ModelState.AddModelError(string.Empty, createResult.Message);

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm sản phẩm '{viewModel.Name}'.", ToastType.Error));
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    [Authorize(Policy = PermissionConstants.ProductEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        ProductViewModel? viewModel = await _productService.GetProductByIdAsync(id);
        if (viewModel == null)
        {
            _logger.LogWarning("Product not found for editing: ID {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy sản phẩm để chỉnh sửa.", ToastType.Error));
            return RedirectToAction(nameof(Index));
        }
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    [Authorize(Policy = PermissionConstants.ProductEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
               new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error));
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _productViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var updateResult = await _productService.UpdateProductAsync(viewModel);
        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? "Cập nhật sản phẩm thành công.", ToastType.Success));
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in updateResult.Errors)
            ModelState.AddModelError(string.Empty, error);
        if (!updateResult.Errors.Any() && !string.IsNullOrEmpty(updateResult.Message))
            ModelState.AddModelError(string.Empty, updateResult.Message);

        TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
            new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật sản phẩm '{viewModel.Name}'.", ToastType.Error));
        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    [Authorize(Policy = PermissionConstants.ProductDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _productService.DeleteProductAsync(id);
        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa sản phẩm thành công.", ToastType.Success));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa sản phẩm.", ToastType.Error));
        }
        return RedirectToAction(nameof(Index));
    }
}

public partial class ProductController
{
    private async Task PopulateViewModelSelectListsAsync(ProductViewModel viewModel)
    {
        viewModel.CategoryOptions = await _categoryService.GetParentCategorySelectListAsync(CategoryType.Product, viewModel.CategoryId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.TagOptions = await _tagService.GetTagSelectListAsync(TagType.Product, viewModel.SelectedTagIds);
    }

    private List<SelectListItem> GetPublishStatusSelectList(PublishStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(PublishStatus))
            .Cast<PublishStatus>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedStatus.HasValue && t == selectedStatus.Value
            })
            .OrderBy(t => t.Text)
            .ToList();
        items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedStatus.HasValue });
        return items;
    }

    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allTextPrefix = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = $"{allTextPrefix}", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }
}