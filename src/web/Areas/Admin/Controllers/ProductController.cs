using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ProductViewModel> _validator;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ProductViewModel> validator,
        ILogger<ProductController> logger)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index(string? searchTerm = null, int? typeId = null, int? brandId = null, PublishStatus? status = null)
    {
        ViewData["PageTitle"] = "Quản lý Sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", "") };

        var query = _context.Set<Product>()
                            .Include(p => p.ProductType)
                            .Include(p => p.Brand)
                            .Include(p => p.Images) // Needed for main image
                            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || (p.ShortDescription != null && p.ShortDescription.Contains(searchTerm)));
        }
        if (typeId.HasValue)
        {
            query = query.Where(p => p.ProductTypeId == typeId.Value);
        }
        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<ProductListItemViewModel>>(products);

        // Load filter data
        ViewBag.ProductTypes = await _context.Set<ProductType>().OrderBy(pt => pt.Name).Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name }).ToListAsync();
        ViewBag.Brands = await _context.Set<Brand>().OrderBy(b => b.Name).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name }).ToListAsync();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = GetStatusDisplayName(s) }).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedTypeId = typeId;
        ViewBag.SelectedBrandId = brandId;
        ViewBag.SelectedStatus = status;

        return View(viewModels);
    }


    // GET: Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm Sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new ProductViewModel
        {
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.8,
            SitemapChangeFrequency = "weekly",
            OgType = "product"
        };

        await LoadDropdownsAsync(viewModel); // Load dropdowns
        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        // Manual Checks before main validation
        if (await _context.Set<Product>().AnyAsync(p => p.Slug == viewModel.Slug))
        {
            ModelState.AddModelError(nameof(ProductViewModel.Slug), "Slug đã tồn tại.");
        }
        // Check Variant SKU uniqueness (within this product submission)
        var duplicateSkus = viewModel.Variants?
                                .Where(v => !v.IsDeleted && !string.IsNullOrWhiteSpace(v.Sku))
                                .GroupBy(v => v.Sku)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key).ToList();
        if (duplicateSkus != null && duplicateSkus.Any())
        {
            ModelState.AddModelError("Variants", $"Các SKU sau bị trùng lặp: {string.Join(", ", duplicateSkus)}");
        }
        // Check if at least one image is marked as main (if images exist)
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError("Images", "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }


        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            _logger.LogWarning("Product creation validation failed.");
            await LoadDropdownsAsync(viewModel); // Reload dropdowns
            ViewData["PageTitle"] = "Thêm Sản phẩm mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

        var product = _mapper.Map<Product>(viewModel);

        // --- MANUAL RELATIONSHIP HANDLING ---
        product.ProductCategories = viewModel.SelectedCategoryIds
                                        .Select(catId => new ProductCategory { CategoryId = catId })
                                        .ToList();
        product.ProductTags = viewModel.SelectedTagIds
                                    .Select(tagId => new ProductTag { TagId = tagId })
                                    .ToList();
        product.Images = _mapper.Map<List<ProductImage>>(viewModel.Images.Where(i => !i.IsDeleted)); // Map only non-deleted images
        product.Variants = _mapper.Map<List<ProductVariant>>(viewModel.Variants.Where(v => !v.IsDeleted)); // Map only non-deleted variants

        _context.Products.Add(product);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving new product.");
            ModelState.AddModelError("", "Không thể lưu sản phẩm. Vui lòng kiểm tra lại dữ liệu (SKU có thể bị trùng với sản phẩm khác?).");
            await LoadDropdownsAsync(viewModel); // Reload dropdowns
            ViewData["PageTitle"] = "Thêm Sản phẩm mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };
            return View(viewModel);
        }

    }


    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        // Eagerly load ALL related data needed for the form
        var product = await _context.Set<Product>()
            .Include(p => p.ProductType)
            .Include(p => p.Brand)
            .Include(p => p.Images.OrderBy(i => i.OrderIndex)) // Order images
            .Include(p => p.Variants.OrderBy(v => v.Name)) // Order variants
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ProductViewModel>(product);
        // SelectedCategoryIds and SelectedTagIds are mapped by AutoMapper profile

        await LoadDropdownsAsync(viewModel); // Load dropdowns

        ViewData["PageTitle"] = "Chỉnh sửa Sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }


    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        // --- Manual Checks ---
        if (await _context.Set<Product>().AnyAsync(p => p.Slug == viewModel.Slug && p.Id != id))
        {
            ModelState.AddModelError(nameof(ProductViewModel.Slug), "Slug đã tồn tại.");
        }
        // Check Variant SKU uniqueness (within submission AND against other existing variants)
        var submittedVariants = viewModel.Variants?.Where(v => !v.IsDeleted).ToList() ?? new List<ProductVariantViewModel>();
        var duplicateSkus = submittedVariants
                               .Where(v => !string.IsNullOrWhiteSpace(v.Sku))
                               .GroupBy(v => v.Sku.ToLowerInvariant()) // Case-insensitive check
                               .Where(g => g.Count() > 1)
                               .Select(g => g.Key).ToList();
        if (duplicateSkus.Any())
        {
            ModelState.AddModelError("Variants", $"Các SKU sau bị trùng lặp trong danh sách nhập: {string.Join(", ", duplicateSkus)}");
        }
        else
        {
            // Check against DB (excluding variants of the current product being edited)
            var skusToCheck = submittedVariants.Where(v => !string.IsNullOrWhiteSpace(v.Sku)).Select(v => v.Sku.ToLowerInvariant()).ToList();
            if (skusToCheck.Any())
            {
                var existingSku = await _context.Set<ProductVariant>()
                                        .Where(v => v.ProductId != id && skusToCheck.Contains(v.Sku.ToLowerInvariant()))
                                        .Select(v => v.Sku)
                                        .FirstOrDefaultAsync();
                if (existingSku != null)
                {
                    ModelState.AddModelError("Variants", $"SKU '{existingSku}' đã tồn tại ở sản phẩm khác.");
                }
            }
        }
        // Check Main Image
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError("Images", "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }


        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            _logger.LogWarning("Product editing validation failed for ID: {ProductId}", id);
            await LoadDropdownsAsync(viewModel); // Reload dropdowns
            ViewData["PageTitle"] = "Chỉnh sửa Sản phẩm";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            // Need to reload image/variant data if returning view? Yes, map again or pass original viewmodel?
            // It's complex. Let's assume the current viewModel is sufficient for re-displaying errors.
            return View(viewModel);
        }


        // --- Load Existing Entity with ALL Related Data for Update ---
        var product = await _context.Set<Product>()
            .Include(p => p.Images)
            .Include(p => p.Variants)
            .Include(p => p.ProductCategories)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return NotFound();

        // --- Map Scalar Properties ---
        _mapper.Map(viewModel, product);

        // --- MANUAL RELATIONSHIP HANDLING (UPDATE/ADD/DELETE) ---

        // Categories
        var currentCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
        var categoriesToAdd = viewModel.SelectedCategoryIds.Except(currentCategoryIds).Select(catId => new ProductCategory { ProductId = id, CategoryId = catId });
        var categoriesToRemove = product.ProductCategories.Where(pc => !viewModel.SelectedCategoryIds.Contains(pc.CategoryId)).ToList();
        _context.ProductCategories.RemoveRange(categoriesToRemove);
        await _context.ProductCategories.AddRangeAsync(categoriesToAdd);

        // Tags (Similar logic)
        var currentTagIds = product.ProductTags.Select(pt => pt.TagId).ToList();
        var tagsToAdd = viewModel.SelectedTagIds.Except(currentTagIds).Select(tagId => new ProductTag { ProductId = id, TagId = tagId });
        var tagsToRemove = product.ProductTags.Where(pt => !viewModel.SelectedTagIds.Contains(pt.TagId)).ToList();
        _context.ProductTags.RemoveRange(tagsToRemove);
        await _context.ProductTags.AddRangeAsync(tagsToAdd);


        // Images
        var imagesToDeleteFromMinio = new List<string>();
        // Delete images marked for deletion or missing from viewModel
        var imageIdsInViewModel = viewModel.Images.Where(i => i.Id > 0 && !i.IsDeleted).Select(i => i.Id).ToList();
        var imagesToRemove = product.Images.Where(dbImg => !imageIdsInViewModel.Contains(dbImg.Id)).ToList();
        foreach (var imgToRemove in imagesToRemove)
        {
            if (!string.IsNullOrEmpty(imgToRemove.ImageUrl)) imagesToDeleteFromMinio.Add(imgToRemove.ImageUrl);
            // Add ThumbnailUrl too if it's different and exists
            if (!string.IsNullOrEmpty(imgToRemove.ThumbnailUrl) && imgToRemove.ThumbnailUrl != imgToRemove.ImageUrl) imagesToDeleteFromMinio.Add(imgToRemove.ThumbnailUrl);
            _context.ProductImages.Remove(imgToRemove); // Remove from DB context
        }
        // Update existing images and add new ones
        foreach (var imgVm in viewModel.Images.Where(i => !i.IsDeleted))
        {
            if (imgVm.Id > 0) // Existing image - Update
            {
                var dbImage = product.Images.FirstOrDefault(i => i.Id == imgVm.Id);
                if (dbImage != null) _mapper.Map(imgVm, dbImage); // Map changes
            }
            else // New image - Add
            {
                var newImage = _mapper.Map<ProductImage>(imgVm);
                newImage.ProductId = id; // Link to product
                product.Images.Add(newImage); // Add to context via navigation property
            }
        }

        // Variants (Similar logic to Images)
        // Delete variants marked for deletion or missing from viewModel
        var variantIdsInViewModel = viewModel.Variants.Where(v => v.Id > 0 && !v.IsDeleted).Select(v => v.Id).ToList();
        var variantsToRemove = product.Variants.Where(dbVar => !variantIdsInViewModel.Contains(dbVar.Id)).ToList();
        _context.ProductVariants.RemoveRange(variantsToRemove); // Remove from DB context

        // Update existing variants and add new ones
        foreach (var varVm in viewModel.Variants.Where(v => !v.IsDeleted))
        {
            if (varVm.Id > 0) // Existing variant - Update
            {
                var dbVariant = product.Variants.FirstOrDefault(v => v.Id == varVm.Id);
                if (dbVariant != null) _mapper.Map(varVm, dbVariant); // Map changes
            }
            else // New variant - Add
            {
                var newVariant = _mapper.Map<ProductVariant>(varVm);
                newVariant.ProductId = id; // Link to product
                product.Variants.Add(newVariant); // Add to context via navigation property
            }
        }


        try
        {
            await _context.SaveChangesAsync(); // Save all DB changes

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id)) return NotFound();
            _logger.LogWarning("Concurrency conflict updating product ID: {ProductId}", id);
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating product ID: {ProductId}", id);
            ModelState.AddModelError("", "Không thể lưu sản phẩm. Vui lòng kiểm tra lại dữ liệu (SKU có thể bị trùng?).");
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
    }


    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Set<Product>()
                                 .Include(p => p.Images) // Include images to delete from storage
                                                         // Include other relations if they block deletion (though cascade should handle most)
                                 .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }

        // Store image paths BEFORE deleting the product from DB
        var imagesToDeleteFromMinio = product.Images?
           .Select(i => i.ImageUrl)
           .Where(url => !string.IsNullOrEmpty(url))
           .Distinct() // Avoid deleting the same path multiple times
           .ToList() ?? new List<string>();
        // Add ThumbnailUrls if they are different
        imagesToDeleteFromMinio.AddRange(product.Images?
                                  .Where(i => !string.IsNullOrEmpty(i.ThumbnailUrl) && i.ThumbnailUrl != i.ImageUrl)
                                  .Select(i => i.ThumbnailUrl!)
                                  .Distinct() ?? Enumerable.Empty<string>());
        imagesToDeleteFromMinio = imagesToDeleteFromMinio.Distinct().ToList(); // Ensure uniqueness again


        _context.Products.Remove(product); // Cascade delete should handle related entities like Images, Variants, Categories, Tags

        try
        {
            await _context.SaveChangesAsync(); // Delete from DB first

            return Json(new { success = true, message = "Xóa sản phẩm thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}. It might be referenced by other entities with RESTRICT constraint (e.g., ProjectProduct, ArticleProduct).", id);
            return Json(new { success = false, message = "Không thể xóa sản phẩm. Sản phẩm có thể đang được liên kết với Dự án hoặc Bài viết." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }


    // --- Helper Methods ---
    private async Task LoadDropdownsAsync(ProductViewModel viewModel)
    {
        viewModel.ProductTypeList = new SelectList(await _context.Set<ProductType>().Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync(), "Id", "Name", viewModel.ProductTypeId);
        viewModel.BrandList = new SelectList(await _context.Set<Brand>().Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync(), "Id", "Name", viewModel.BrandId);

        // Load categories specific to Products
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Product).OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        // Load tags specific to Products
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Product).OrderBy(t => t.Name).ToListAsync(), "Id", "Name");

        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus))
                                               .Cast<PublishStatus>()
                                               .Select(e => new { Value = e, Text = GetStatusDisplayName(e) }),
                                               "Value", "Text", viewModel.Status);
    }

    private string GetStatusDisplayName(PublishStatus status)
    {
        // Consider using Display attribute or resource file
        return status switch
        {
            PublishStatus.Published => "Đã xuất bản",
            PublishStatus.Draft => "Bản nháp",
            PublishStatus.Scheduled => "Đã lên lịch",
            PublishStatus.Archived => "Lưu trữ",
            _ => status.ToString()
        };
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _context.Set<Product>().AnyAsync(e => e.Id == id);
    }
}