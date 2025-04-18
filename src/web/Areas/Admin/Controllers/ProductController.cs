using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Product;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index(string? searchTerm = null, int? brandId = null, PublishStatus? status = null, int page = 1, int pageSize = 15)
    {
        ViewData["Title"] = "Quản lý Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Sản phẩm";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Product>()
                            .Include(p => p.Brand)
                            .Include(p => p.Images.OrderBy(i => i.OrderIndex))
                            .AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm)
                                  || (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm))
                                  || p.Variants.Any(v => v.Sku.ToLower().Contains(lowerSearchTerm))
                                  );
        }
        if (brandId.HasValue && brandId > 0)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        // Ordering and Pagination
        var productsPaged = await query
            .OrderByDescending(p => p.IsFeatured) // Featured first
            .ThenByDescending(p => p.CreatedAt)  // Then by creation date
            .ProjectTo<ProductListItemViewModel>(_mapper.ConfigurationProvider) // Project efficiently
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter data
        await LoadFilterDropdownsAsync(brandId, status);

        ViewBag.SearchTerm = searchTerm;
        // Selected IDs/Status are loaded into ViewBag by LoadFilterDropdownsAsync

        return View(productsPaged); // Pass paged list
    }


    // GET: Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Thêm Sản phẩm mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };

        var viewModel = new ProductViewModel
        {
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.8,
            SitemapChangeFrequency = "weekly",
            OgType = "product",
            Images = new List<ProductImageViewModel>(), // Initialize lists
            Variants = new List<ProductVariantViewModel>() // Initialize lists
        };

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns
        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        // --- Perform manual/complex checks before ModelState.IsValid ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: false);

        // Rely on middleware for remaining validation (including FluentValidation DB checks like unique slug)
        if (ModelState.IsValid)
        {
            var product = _mapper.Map<Product>(viewModel);

            product.ProductTags = viewModel.SelectedTagIds? // Check for null
                                    .Select(tagId => new ProductTag { TagId = tagId })
                                    .ToList() ?? new List<ProductTag>(); // Default to empty list

            // Map only non-deleted Images and Variants from ViewModel
            product.Images = _mapper.Map<List<ProductImage>>(viewModel.Images?.Where(i => !i.IsDeleted).ToList() ?? new List<ProductImageViewModel>());
            product.Variants = _mapper.Map<List<ProductVariant>>(viewModel.Variants?.Where(v => !v.IsDeleted).ToList() ?? new List<ProductVariantViewModel>());

            // Set audit fields if needed
            // product.CreatedBy = User.Identity?.Name;

            _context.Products.Add(product);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product '{Name}' (ID: {ProductId}) created successfully by {User}.", product.Name, product.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Thêm sản phẩm '{product.Name}' thành công!"; // Standard key
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) // Catch specific DB errors
            {
                _logger.LogError(ex, "Database error saving new product '{Name}'. Check constraints.", viewModel.Name);
                // Check inner exception for constraint violation if needed
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: products.slug", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                }
                else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_variants.sku", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError("Variants", "Một hoặc nhiều SKU đã tồn tại ở sản phẩm khác.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving new product '{Name}'.", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu sản phẩm.");
            }
        }
        else
        {
            _logger.LogWarning("Product creation failed for '{Name}'. Model state is invalid.", viewModel.Name);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Thêm Sản phẩm mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Thêm Sản phẩm mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ("Thêm mới", "") };
        // Repopulate Images/Variants in the view model if needed after validation failure (e.g., if using hidden inputs and they get cleared)
        // viewModel.Images = viewModel.Images ?? new List<ProductImageViewModel>();
        // viewModel.Variants = viewModel.Variants ?? new List<ProductVariantViewModel>();
        return View(viewModel);
    }


    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        // Eagerly load ALL related data needed for the form
        var product = await _context.Set<Product>()
            .Include(p => p.Brand)
            .Include(p => p.Images.OrderBy(i => i.OrderIndex)) // Order images
            .Include(p => p.Variants.OrderBy(v => v.Name)) // Order variants
            .Include(p => p.CategoryId)
            .Include(p => p.ProductTags)
            .AsNoTracking() // Use AsNoTracking for reading into ViewModel
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Edit GET: Product with ID {ProductId} not found.", id);
            return NotFound();
        }

        var viewModel = _mapper.Map<ProductViewModel>(product);
        // SelectedCategoryIds and SelectedTagIds are mapped by AutoMapper profile

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns

        ViewData["Title"] = "Chỉnh sửa Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Sản phẩm: {product.Name}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(product.Name)}", "") };

        return View(viewModel);
    }


    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        // --- Perform manual/complex checks before ModelState.IsValid ---
        await ValidateRelationsAndUniquenessAsync(viewModel, isEdit: true);

        if (ModelState.IsValid)
        {
            // --- Load Existing TRACKED Entity with ALL Related Data for Update ---
            var product = await _context.Set<Product>()
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Edit POST: Product with ID {ProductId} not found for update.", id);
                TempData["error"] = "Không tìm thấy sản phẩm để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            // --- Map Scalar Properties from ViewModel to Entity ---
            _mapper.Map(viewModel, product);

            try
            {
                // --- MANUAL RELATIONSHIP HANDLING (UPDATE/ADD/DELETE) ---
                UpdateProductTags(product, viewModel.SelectedTagIds);
                UpdateProductImages(product, viewModel.Images);
                UpdateProductVariants(product, viewModel.Variants);

                // Set audit fields if needed
                // product.UpdatedBy = User.Identity?.Name;

                await _context.SaveChangesAsync(); // Save all changes

                _logger.LogInformation("Product '{Name}' (ID: {ProductId}) updated successfully by {User}.", product.Name, id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật sản phẩm '{product.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency error occurred while editing Product ID {ProductId}.", id);
                TempData["error"] = "Lỗi: Xung đột dữ liệu. Sản phẩm này có thể đã được cập nhật bởi người khác.";
            }
            catch (DbUpdateException ex) // Catch specific DB errors
            {
                _logger.LogError(ex, "Database error updating product ID {ProductId}. Check constraints.", id);
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: products.slug", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại.");
                }
                else if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: product_variants.sku", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    ModelState.AddModelError("Variants", "Một hoặc nhiều SKU đã tồn tại ở sản phẩm khác.");
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi cập nhật sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product ID: {ProductId}.", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật sản phẩm.");
            }
        }
        else
        {
            _logger.LogWarning("Product editing failed for ID: {ProductId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Chỉnh sửa Sản phẩm - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Sản phẩm: {viewModel.Name}"; // Use name from VM
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Sản phẩm", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Name)}", "") };
        // Ensure Images/Variants list is still populated in the view model
        // If using hidden fields, they should persist. If reloaded from DB, map again.
        return View(viewModel);
    }


    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        // Include relations that have RESTRICT constraints if any (e.g., ProjectProduct, ArticleProduct)
        // Cascade should handle Images, Variants, Categories, Tags automatically.
        var product = await _context.Set<Product>()
                                 // .Include(p => p.ProjectProducts) // Uncomment if deletion restricted by Projects
                                 // .Include(p => p.ArticleProducts) // Uncomment if deletion restricted by Articles
                                 .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Delete POST: Product with ID {ProductId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }

        // --- Optional: Check for restricting relationships BEFORE attempting delete ---
        // if (product.ProjectProducts != null && product.ProjectProducts.Any())
        // {
        //     _logger.LogWarning("Attempted to delete Product ID {ProductId} referenced by Projects.", id);
        //     return Json(new { success = false, message = "Không thể xóa sản phẩm vì đang được liên kết với Dự án." });
        // }
        // if (product.ArticleProducts != null && product.ArticleProducts.Any())
        // {
        //      _logger.LogWarning("Attempted to delete Product ID {ProductId} referenced by Articles.", id);
        //     return Json(new { success = false, message = "Không thể xóa sản phẩm vì đang được liên kết với Bài viết." });
        // }
        // --- End Optional Checks ---


        try
        {
            string productName = product.Name;
            // NOTE: Deleting Images from MinIO should happen AFTER successful DB deletion
            // or via a background job/event listener to avoid orphaned files if DB delete fails.
            // We won't implement MinIO delete here for brevity.

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(); // DB delete

            _logger.LogInformation("Product '{ProductName}' (ID: {ProductId}) deleted successfully by {User}.", productName, id, User.Identity?.Name ?? "Unknown");
            // TODO: Implement logic to delete associated images from MinIO storage here or queue a job.

            return Json(new { success = true, message = $"Xóa sản phẩm '{productName}' thành công." });
        }
        catch (DbUpdateException ex) // Catch potential foreign key constraint violations (RESTRICT)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}. Potential RESTRICT constraint violation.", id);
            // Provide a more specific message if possible based on the exception details
            if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                return Json(new { success = false, message = "Không thể xóa sản phẩm. Sản phẩm có thể đang được liên kết với dữ liệu khác (ví dụ: Dự án, Bài viết)." });
            }
            return Json(new { success = false, message = "Lỗi cơ sở dữ liệu khi xóa sản phẩm." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Product ID {ProductId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }


    // --- Helper Methods ---

    // Helper to load dropdowns for Create/Edit views
    private async Task LoadRelatedDataForFormAsync(ProductViewModel viewModel)
    {
        viewModel.BrandList = new SelectList(await _context.Set<Brand>().Where(t => t.IsActive).OrderBy(t => t.Name).Select(t => new { t.Id, t.Name }).ToListAsync(), "Id", "Name", viewModel.BrandId);
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Product).OrderBy(c => c.Name).Select(t => new { t.Id, t.Name }).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Product).OrderBy(t => t.Name).Select(t => new { t.Id, t.Name }).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus))
                                               .Cast<PublishStatus>()
                                               .Select(e => new { Value = e, Text = e.GetDisplayName() }), // Use shared extension
                                               "Value", "Text", viewModel.Status);
    }

    // Helper to load dropdowns for Index filter controls
    private async Task LoadFilterDropdownsAsync(int? brandId, PublishStatus? status)
    {
        ViewBag.Brands = await _context.Set<Brand>().Where(b => b.IsActive).OrderBy(b => b.Name).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name, Selected = b.Id == brandId }).ToListAsync();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == status }).ToList(); // Use shared extension
        ViewBag.SelectedBrandId = brandId;
        ViewBag.SelectedStatus = status;
    }

    // Helper for complex validation before main model validation
    private async Task ValidateRelationsAndUniquenessAsync(ProductViewModel viewModel, bool isEdit, CancellationToken cancellationToken = default)
    {
        // 1. Check Main Image ... (Keep this logic as before) ...
        if (viewModel.Images != null && viewModel.Images.Any(i => !i.IsDeleted) && !viewModel.Images.Any(i => i.IsMain && !i.IsDeleted))
        {
            ModelState.AddModelError(nameof(viewModel.Images), "Vui lòng chọn một ảnh làm ảnh đại diện chính.");
        }
        // Ensure only one image is main
        if (viewModel.Images != null && viewModel.Images.Count(i => i.IsMain && !i.IsDeleted) > 1)
        {
            ModelState.AddModelError(nameof(viewModel.Images), "Chỉ được chọn một ảnh làm ảnh đại diện chính.");
        }

        // 2. Check Variant SKU uniqueness (within submission) ... (Keep this logic as before) ...
        var submittedVariants = viewModel.Variants?.Where(v => !v.IsDeleted).ToList() ?? new List<ProductVariantViewModel>();
        var duplicateSkusInSubmission = submittedVariants
                               .Where(v => !string.IsNullOrWhiteSpace(v.Sku))
                               .GroupBy(v => v.Sku.Trim().ToLowerInvariant()) // Keep Invariant for C# grouping logic
                               .Where(g => g.Count() > 1)
                               .Select(g => g.Key).ToList();
        if (duplicateSkusInSubmission.Any())
        {
            ModelState.AddModelError("Variants", $"Các SKU sau bị trùng lặp trong danh sách biến thể: {string.Join(", ", duplicateSkusInSubmission)}");
        }

        // 3. Check Variant SKU uniqueness (against DB)
        var skusToCheck = submittedVariants
                          .Where(v => !string.IsNullOrWhiteSpace(v.Sku))
                          .Select(v => v.Sku.Trim().ToLower()) // **** CHANGED HERE: Use ToLower() ****
                          .Distinct()
                          .ToList();

        if (skusToCheck.Any())
        {
            // Find existing SKUs in the DB that belong to OTHER products
            // Use ToLower() for the database-side comparison as well
            var query = _context.Set<ProductVariant>()
                            .Where(v => skusToCheck.Contains(v.Sku.ToLower())); // **** CHANGED HERE: Use ToLower() ****

            if (isEdit)
            {
                query = query.Where(v => v.ProductId != viewModel.Id);
            }

            // Pass CancellationToken to async method
            var existingDbSku = await query.Select(v => v.Sku).FirstOrDefaultAsync(cancellationToken);

            if (existingDbSku != null)
            {
                ModelState.AddModelError("Variants", $"SKU '{existingDbSku}' đã tồn tại ở một sản phẩm khác.");
            }
        }
    }

    // --- Helpers for M-M and O-M Updates (called from Edit POST) ---


    private void UpdateProductTags(Product product, List<int>? selectedTagIds)
    {
        selectedTagIds ??= new List<int>();

        // Remove tags no longer selected
        var tagsToRemove = product.ProductTags
                                 .Where(pt => !selectedTagIds.Contains(pt.TagId))
                                 .ToList();
        if (tagsToRemove.Any())
        {
            _context.ProductTags.RemoveRange(tagsToRemove);
        }

        // Add newly selected tags
        var existingTagIds = product.ProductTags.Select(pt => pt.TagId);
        var tagIdsToAdd = selectedTagIds.Except(existingTagIds).ToList();
        if (tagIdsToAdd.Any())
        {
            foreach (var tagId in tagIdsToAdd)
            {
                product.ProductTags.Add(new ProductTag { TagId = tagId });
            }
        }
    }

    private void UpdateProductImages(Product product, List<ProductImageViewModel>? imageVMs)
    {
        imageVMs ??= new List<ProductImageViewModel>();

        // Delete images marked for deletion in VM or not present in VM anymore
        var vmImageIds = imageVMs.Where(i => i.Id > 0).Select(i => i.Id).ToList();
        var dbImagesToRemove = product.Images
                                    .Where(dbImg => !vmImageIds.Contains(dbImg.Id) || imageVMs.First(vm => vm.Id == dbImg.Id).IsDeleted)
                                    .ToList();
        if (dbImagesToRemove.Any())
        {
            // TODO: Queue these image URLs for deletion from MinIO storage AFTER SaveChangesAsync succeeds
            // var urlsToDelete = dbImagesToRemove.Select(img => img.ImageUrl).Where(url => !string.IsNullOrEmpty(url)).Distinct();
            _context.ProductImages.RemoveRange(dbImagesToRemove);
        }


        // Update existing or add new images
        foreach (var imgVm in imageVMs.Where(i => !i.IsDeleted))
        {
            if (imgVm.Id > 0) // Update existing
            {
                var dbImage = product.Images.FirstOrDefault(i => i.Id == imgVm.Id);
                if (dbImage != null)
                {
                    // Map only the editable fields from VM to existing DB entity
                    dbImage.AltText = imgVm.AltText;
                    dbImage.Title = imgVm.Title;
                    dbImage.OrderIndex = imgVm.OrderIndex;
                    dbImage.IsMain = imgVm.IsMain;
                    // Don't map ImageUrl/ThumbnailUrl here as they are set on upload
                }
            }
            else // Add new
            {
                // Only add if ImageUrl is actually set (meaning upload was successful or path provided)
                if (!string.IsNullOrWhiteSpace(imgVm.ImageUrl))
                {
                    var newImage = _mapper.Map<ProductImage>(imgVm);
                    // ProductId is set automatically by EF Core when adding to navigation property
                    product.Images.Add(newImage);
                }
                else
                {
                    _logger.LogWarning("Skipping adding new image because ImageUrl is empty. VM data: {@ImageViewModel}", imgVm);
                }
            }
        }
    }

    private void UpdateProductVariants(Product product, List<ProductVariantViewModel>? variantVMs)
    {
        variantVMs ??= new List<ProductVariantViewModel>();

        // Delete variants marked for deletion in VM or not present in VM anymore
        var vmVariantIds = variantVMs.Where(v => v.Id > 0).Select(v => v.Id).ToList();
        var dbVariantsToRemove = product.Variants
                                   .Where(dbVar => !vmVariantIds.Contains(dbVar.Id) || variantVMs.First(vm => vm.Id == dbVar.Id).IsDeleted)
                                   .ToList();
        if (dbVariantsToRemove.Any())
        {
            _context.ProductVariants.RemoveRange(dbVariantsToRemove);
        }


        // Update existing or add new variants
        foreach (var varVm in variantVMs.Where(v => !v.IsDeleted))
        {
            if (varVm.Id > 0) // Update existing
            {
                var dbVariant = product.Variants.FirstOrDefault(v => v.Id == varVm.Id);
                if (dbVariant != null)
                {
                    // Map all editable fields
                    _mapper.Map(varVm, dbVariant);
                }
            }
            else // Add new
            {
                var newVariant = _mapper.Map<ProductVariant>(varVm);
                // ProductId is set automatically by EF Core
                product.Variants.Add(newVariant);
            }
        }
    }

    private string Truncate(string value, int maxLength = 30) // Helper for short display
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}