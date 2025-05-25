using System.Text.Json;
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
using shared.Models;
using web.Areas.Admin.Validators;
using web.Areas.Admin.ViewModels;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class ProductVariationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductVariationController> _logger;

    public ProductVariationController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductVariationController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/ProductVariation
    [Authorize(Policy = PermissionConstants.ProductVariationView)]
    public async Task<IActionResult> Index(int productId, ProductVariationFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
            return RedirectToAction("Index", "Product");
        }

        filter ??= new ProductVariationFilterViewModel { ProductId = productId };
        filter.ProductId = productId;

        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<ProductVariation> query = _context.Set<ProductVariation>()
                                                     .Where(v => v.ProductId == productId)
                                                     .Include(v => v.ProductVariationAttributeValues!)
                                                        .ThenInclude(pvav => pvav.AttributeValue)
                                                            .ThenInclude(av => av.Attribute)
                                                     .AsNoTracking();

        if (filter.IsActive.HasValue)
        {
            query = query.Where(v => v.IsActive == filter.IsActive.Value);
        }

        if (filter.IsDefault.HasValue)
        {
            query = query.Where(v => v.IsDefault == filter.IsDefault.Value);
        }

        query = query.OrderByDescending(v => v.IsDefault)
                     .ThenBy(v => v.CreatedAt);

        var variationsPaged = await query.ProjectTo<ProductVariationListItemViewModel>(_mapper.ConfigurationProvider)
                                        .ToPagedListAsync(pageNumber, currentPageSize);


        filter.StatusOptions = GetYesNoSelectList(filter.IsActive, "Tất cả");
        filter.IsDefaultOptions = GetYesNoSelectList(filter.IsDefault, "Tất cả");

        ProductVariationIndexViewModel viewModel = new()
        {
            ProductId = productId,
            ProductName = product.Name,
            Variations = variationsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/ProductVariation/Create
    [Authorize(Policy = PermissionConstants.ProductVariationCreate)]
    public async Task<IActionResult> Create(int productId)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy sản phẩm để tạo biến thể.";
            return RedirectToAction("Index", "Product");
        }

        ProductVariationViewModel viewModel = new()
        {
            ProductId = productId,
            ProductName = product.Name,
            Price = 0,
            StockQuantity = 0,
            IsActive = true,
            IsDefault = false,
        };

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Create
    [Authorize(Policy = PermissionConstants.ProductVariationCreate)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductVariationViewModel viewModel)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == viewModel.ProductId);
        if (product == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Sản phẩm cha không tồn tại.", ToastType.Error)
            );
            return RedirectToAction("Index", "Product");
        }
        viewModel.ProductName = product.Name;

        var result = await new ProductVariationViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        if (viewModel.IsDefault)
        {
            await SetOtherVariationsNonDefaultAsync(viewModel.ProductId, 0);
        }

        var variation = _mapper.Map<ProductVariation>(viewModel);

        if (viewModel.SelectedAttributeValueIds != null && viewModel.SelectedAttributeValueIds.Any())
        {
            variation.ProductVariationAttributeValues = viewModel.SelectedAttributeValueIds
                .Select(avId => new ProductVariationAttributeValue { AttributeValueId = avId })
                .ToList();
        }

        _context.Add(variation);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Đã thêm biến thể thành công cho sản phẩm '{product.Name}'.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { productId = viewModel.ProductId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi thêm biến thể cho sản phẩm ID: {ProductId}.", viewModel.ProductId);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi lưu biến thể.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/ProductVariation/Edit/5
    [Authorize(Policy = PermissionConstants.ProductVariationEdit)]
    public async Task<IActionResult> Edit(int id)
    {
        var variation = await _context.Set<ProductVariation>()
                                      .Include(v => v.Product)
                                      .Include(v => v.ProductVariationAttributeValues!)
                                          .ThenInclude(pvav => pvav.AttributeValue)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(v => v.Id == id);

        if (variation == null)
        {
            _logger.LogWarning("Không tìm thấy biến thể để chỉnh sửa. ID: {Id}", id);
            TempData["ErrorMessage"] = "Không tìm thấy biến thể.";
            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }

        ProductVariationViewModel viewModel = _mapper.Map<ProductVariationViewModel>(variation);
        viewModel.ProductName = variation.Product.Name;

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Edit/5
    [Authorize(Policy = PermissionConstants.ProductVariationEdit)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductVariationViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Yêu cầu chỉnh sửa không hợp lệ.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == viewModel.ProductId);
        if (product == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Sản phẩm cha không tồn tại.", ToastType.Error)
            );
            return RedirectToAction("Index", "Product");
        }
        viewModel.ProductName = product.Name;

        var result = await new ProductVariationViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var variationToUpdate = await _context.Set<ProductVariation>()
                                              .Include(v => v.ProductVariationAttributeValues)
                                              .FirstOrDefaultAsync(v => v.Id == id);

        if (variationToUpdate == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy biến thể để cập nhật.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        if (viewModel.IsDefault)
        {
            await SetOtherVariationsNonDefaultAsync(viewModel.ProductId, viewModel.Id);
        }

        _mapper.Map(viewModel, variationToUpdate);

        UpdateProductVariationRelationships(variationToUpdate, viewModel.SelectedAttributeValueIds);

        try
        {
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Đã cập nhật biến thể thành công cho sản phẩm '{product.Name}'.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index), new { productId = viewModel.ProductId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật biến thể ID: {Id}.", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật biến thể.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/ProductVariation/Delete/5
    [Authorize(Policy = PermissionConstants.ProductVariationDelete)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var variation = await _context.Set<ProductVariation>().FirstOrDefaultAsync(v => v.Id == id);
        if (variation == null)
        {
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không tìm thấy biến thể.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }

        var productName = await _context.Products.AsNoTracking()
                                                 .Where(p => p.Id == variation.ProductId)
                                                 .Select(p => p.Name)
                                                 .FirstOrDefaultAsync();

        try
        {
            _context.Remove(variation);
            await _context.SaveChangesAsync();
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Thành công", $"Đã xóa biến thể thành công cho sản phẩm '{productName}'.", ToastType.Success)
            );
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa biến thể ID: {Id}.", id);
            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Đã xảy ra lỗi không mong muốn khi xóa biến thể.", ToastType.Error)
            );
            return RedirectToAction(nameof(Index));
        }
    }
}

public partial class ProductVariationController
{
    private async Task PopulateViewModelSelectListsAsync(ProductVariationViewModel viewModel)
    {
        viewModel.AttributeValueOptions = await LoadAttributeValueSelectListAsync(viewModel.ProductId, viewModel.SelectedAttributeValueIds);
    }

    private async Task<List<SelectListItem>> LoadAttributeValueSelectListAsync(int productId, List<int>? selectedValues = null)
    {
        var productAttributeIds = await _context.ProductAttributes
            .Where(pa => pa.ProductId == productId)
            .Select(pa => pa.AttributeId)
            .ToListAsync();

        if (!productAttributeIds.Any())
        {
            return new List<SelectListItem>();
        }

        var attributeValues = await _context.AttributeValues
                          .Where(av => productAttributeIds.Contains(av.AttributeId))
                          .Include(av => av.Attribute)
                          .AsNoTracking()
                          .Select(av => new { av.Id, av.Value, AttributeName = av.Attribute!.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
        };

        items.AddRange(attributeValues.Select(av => new SelectListItem
        {
            Value = av.Id.ToString(),
            Text = $"{av.AttributeName}: {av.Value}",
            Selected = selectedValues != null && selectedValues.Contains(av.Id)
        }));

        return items;
    }

    private async Task SetOtherVariationsNonDefaultAsync(int productId, int currentVariationId = 0)
    {
        var otherDefaultVariations = await _context.ProductVariations
            .Where(v => v.ProductId == productId && v.Id != currentVariationId && v.IsDefault)
            .ToListAsync();

        foreach (var variation in otherDefaultVariations)
        {
            variation.IsDefault = false;
            _context.Entry(variation).State = EntityState.Modified;
        }
    }

    private void UpdateProductVariationRelationships(
        ProductVariation variation,
        List<int>? selectedAttributeValueIds)
    {
        var existingValueIds = variation.ProductVariationAttributeValues?.Select(pvav => pvav.AttributeValueId).ToList() ?? new List<int>();
        var valueIdsToAdd = selectedAttributeValueIds?.Except(existingValueIds).ToList() ?? new List<int>();
        var valueIdsToRemove = existingValueIds.Except(selectedAttributeValueIds ?? new List<int>()).ToList();

        foreach (var valueId in valueIdsToRemove)
        {
            var pvav = variation.ProductVariationAttributeValues!.First(pa => pa.AttributeValueId == valueId);
            _context.Remove(pvav);
        }

        foreach (var valueId in valueIdsToAdd)
        {
            variation.ProductVariationAttributeValues ??= new List<ProductVariationAttributeValue>();
            variation.ProductVariationAttributeValues.Add(new ProductVariationAttributeValue { ProductVariation = variation, AttributeValueId = valueId });
        }
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