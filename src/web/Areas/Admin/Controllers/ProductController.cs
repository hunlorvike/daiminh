using AutoMapper;
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
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;
    private readonly IValidator<ProductViewModel> _productViewModelValidator;

    public ProductController(
        IProductService productService,
        IMapper mapper,
        ILogger<ProductController> logger,
        IValidator<ProductViewModel> productViewModelValidator)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productViewModelValidator = productViewModelValidator ?? throw new ArgumentNullException(nameof(productViewModelValidator));
    }

    // GET: Admin/Product
    [Authorize(Policy = PermissionConstants.ProductView)]
    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ProductFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IPagedList<ProductListItemViewModel> productsPaged = await _productService.GetPagedProductsAsync(filter, pageNumber, currentPageSize);

        filter.CategoryOptions = await _productService.GetProductCategorySelectListAsync(filter.CategoryId);
        filter.BrandOptions = await _productService.GetBrandSelectListAsync(filter.BrandId);
        filter.StatusOptions = GetPublishStatusSelectList(filter.Status);
        filter.IsFeaturedOptions = GetYesNoSelectList(filter.IsFeatured, "Tất cả");

        ProductIndexViewModel viewModel = new()
        {
            Products = productsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Product/Create
    [Authorize(Policy = PermissionConstants.ProductCreate)]
    public async Task<IActionResult> Create()
    {
        ProductViewModel viewModel = new()
        {
            IsFeatured = false,
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.5,
            SitemapChangeFrequency = "monthly",
            Images = new List<ProductImageViewModel>(),
            StatusOptions = GetPublishStatusSelectList(PublishStatus.Draft)
        };
        await _productService.PopulateProductViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Product/Create
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

            await _productService.PopulateProductViewModelSelectListsAsync(viewModel);
            viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);

            return View(viewModel);
        }

        var createResult = await _productService.CreateProductAsync(viewModel);

        if (createResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", createResult.Message ?? "Đã thêm sản phẩm thành công.", ToastType.Success)
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
                else if (error.Contains("Danh mục", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
                }
                else if (error.Contains("Thương hiệu", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.BrandId), error);
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
                new ToastData("Lỗi", createResult.Message ?? $"Không thể thêm sản phẩm '{viewModel.Name}'.", ToastType.Error)
            );
            await _productService.PopulateProductViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }
    }

    // GET: Admin/Product/Edit/5
    [Authorize(Policy = PermissionConstants.ProductEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        ProductViewModel? viewModel = await _productService.GetProductByIdAsync(id);

        if (viewModel == null)
        {
            _logger.LogWarning("Sản phẩm không tồn tại khi chỉnh sửa. ID: {Id}", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                 new ToastData("Lỗi", "Không tìm thấy sản phẩm để chỉnh sửa.", ToastType.Error)
             );
            return RedirectToAction(nameof(Index));
        }
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        return View(viewModel);
    }

    // POST: Admin/Product/Edit/5
    [Authorize(Policy = PermissionConstants.ProductEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var validationResult = await _productViewModelValidator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await _productService.PopulateProductViewModelSelectListsAsync(viewModel);
            viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
            return View(viewModel);
        }

        var updateResult = await _productService.UpdateProductAsync(viewModel);

        if (updateResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", updateResult.Message ?? $"Đã cập nhật sản phẩm '{viewModel.Name}' thành công.", ToastType.Success)
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
                else if (error.Contains("Danh mục", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), error);
                }
                else if (error.Contains("Thương hiệu", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(viewModel.BrandId), error);
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
                new ToastData("Lỗi", updateResult.Message ?? $"Không thể cập nhật sản phẩm '{viewModel.Name}'.", ToastType.Error)
            );
            await _productService.PopulateProductViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }
    }

    // POST: Admin/Product/Delete/5
    [Authorize(Policy = PermissionConstants.ProductDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteResult = await _productService.DeleteProductAsync(id);

        if (deleteResult.Success)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", deleteResult.Message ?? "Xóa sản phẩm thành công.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", deleteResult.Message ?? "Không thể xóa sản phẩm.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
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

        if (!selectedStatus.HasValue)
        {
            items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = true });
        }
        else
        {
            items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái" });
        }
        return items;
    }

    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allText = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = allText, Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }
}