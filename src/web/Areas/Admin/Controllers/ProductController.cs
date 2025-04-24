using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Tags;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Product;
using X.PagedList;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;
    private readonly IValidator<ProductViewModel> _validator;

    public ProductController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductController> logger,
        IValidator<ProductViewModel> validator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ProductFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<Product> query = _context.Set<Product>()
                                             .Include(p => p.Brand) // Include Brand for display/filtering
                                             .Include(p => p.Category) // Include Category for display/filtering
                                             .Include(p => p.Images) // Include Images for main image/count
                                             .Include(p => p.ProductTags) // Include for TagCount
                                             .Include(p => p.ArticleProducts) // Include for ArticleCount
                                             .AsNoTracking(); // Use AsNoTracking for read operations

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm) ||
                                     (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm)) || // Searching large text might be slow
                                     (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Origin != null && p.Origin.ToLower().Contains(lowerSearchTerm)));
            // Avoid searching large text fields unless necessary for performance
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == filter.BrandId.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == filter.IsActive.Value);
        }


        query = query.OrderBy(p => p.Name); // Default order, adjust as needed

        // Execute query and get the list before mapping and pagination
        var filteredProducts = await query.ToListAsync();

        // Map entities to list item view models
        var productVMs = _mapper.Map<List<ProductListItemViewModel>>(filteredProducts);

        // Apply pagination on the list of view models
        IPagedList<ProductListItemViewModel> productsPaged = productVMs.ToPagedList(pageNumber, currentPageSize);

        // Populate filter options
        filter.StatusOptions = GetPublishStatusSelectList(filter.Status);
        filter.CategoryOptions = await LoadCategorySelectListAsync(filter.CategoryId);
        filter.BrandOptions = await LoadBrandSelectListAsync(filter.BrandId);
        filter.IsFeaturedOptions = GetYesNoAllSelectList(filter.IsFeatured);
        filter.IsActiveOptions = GetStatusSelectList(filter.IsActive);

        ProductIndexViewModel viewModel = new()
        {
            Products = productsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/Product/Create
    public async Task<IActionResult> Create()
    {
        ProductViewModel viewModel = new()
        {
            IsActive = true,
            IsFeatured = false,
            Status = PublishStatus.Draft,
            Seo = new ViewModels.Shared.SeoViewModel // Defaults for SEO
            {
                SitemapPriority = 0.5,
                SitemapChangeFrequency = "monthly"
            },
            Images = new List<ProductImageViewModel>() // Initialize empty image list
        };

        // Populate dropdowns/select lists
        viewModel.CategoryOptions = await LoadCategorySelectListAsync(viewModel.CategoryId);
        viewModel.BrandOptions = await LoadBrandSelectListAsync(viewModel.BrandId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.AvailableTags = await LoadAvailableTagsAsync();
        viewModel.AvailableArticles = await LoadAvailableArticlesAsync();

        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        // Explicitly validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
        }

        // Check ModelState after FluentValidation results are added
        // (Model binding errors are also included in ModelState)
        if (ModelState.IsValid)
        {
            Product product = _mapper.Map<Product>(viewModel);

            // Handle nested ProductImages collection
            // On create, all images in the ViewModel are new
            product.Images = viewModel.Images
                                      .Where(img => !img._Delete) // Only add images not marked for deletion
                                      .Select(imgVm => _mapper.Map<ProductImage>(imgVm))
                                      .ToList();
            // Ensure ProductId is set for new images (handled by EF relationships if added to collection before SaveChanges)


            // Handle related entities (ProductTag, ArticleProduct) manually
            if (viewModel.SelectedTagIds != null && viewModel.SelectedTagIds.Any())
            {
                product.ProductTags = viewModel.SelectedTagIds
                                               .Distinct() // Ensure no duplicate tags
                                               .Select(tagId => new ProductTag { TagId = tagId, Product = product }) // Link back to product
                                               .ToList();
            }
            if (viewModel.SelectedArticleIds != null && viewModel.SelectedArticleIds.Any())
            {
                product.ArticleProducts = viewModel.SelectedArticleIds
                                                  .Distinct() // Ensure no duplicate articles
                                                  .Select(articleId => new ArticleProduct { ArticleId = articleId, Product = product, OrderIndex = 0 }) // Link back to product
                                                  .ToList();
            }

            // Set CreatedAt/UpdatedAt if not handled by DbContext interceptors
            // product.CreatedAt = DateTime.UtcNow;
            // product.UpdatedAt = DateTime.UtcNow;


            _context.Add(product); // Add the product, related entities in collections will be added too

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product created successfully: {Name} (ID: {Id})", product.Name, product.Id);
                TempData["SuccessMessage"] = $"Đã thêm sản phẩm '{product.Name}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating product: {Name}", viewModel.Name);
                // Check for unique slug violation
                if (ex.InnerException?.Message.Contains("idx_products_slug", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("products.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại, vui lòng chọn slug khác.");
                }
                // Check for foreign key violations (e.g., non-existent Category, Brand, Tag, Article IDs)
                else if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError("", "Lỗi liên kết dữ liệu. Vui lòng kiểm tra lại danh mục, thương hiệu, thẻ, hoặc bài viết liên quan.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating product: {Name}", viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu sản phẩm.");
            }

            // If validation or save failed, reload dropdowns/select lists before returning view
            viewModel.CategoryOptions = await LoadCategorySelectListAsync(viewModel.CategoryId);
            viewModel.BrandOptions = await LoadBrandSelectListAsync(viewModel.BrandId);
            viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
            viewModel.AvailableTags = await LoadAvailableTagsAsync();
            viewModel.AvailableArticles = await LoadAvailableArticlesAsync();
        }

        return View(viewModel);
    }


    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Product? product = await _context.Set<Product>()
                                           .Include(p => p.Brand) // Include Brand
                                           .Include(p => p.Category) // Include Category
                                           .Include(p => p.Images.OrderBy(img => img.OrderIndex)) // Include Images and order them for display
                                           .Include(p => p.ProductTags) // Include ProductTags for mapping SelectedTagIds
                                           .Include(p => p.ArticleProducts) // Include ArticleProducts for mapping SelectedArticleIds
                                           .AsNoTracking() // Use AsNoTracking for GET
                                           .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product not found for editing: ID {Id}", id);
            return NotFound();
        }

        ProductViewModel viewModel = _mapper.Map<ProductViewModel>(product);

        // Populate dropdowns/select lists
        viewModel.CategoryOptions = await LoadCategorySelectListAsync(viewModel.CategoryId);
        viewModel.BrandOptions = await LoadBrandSelectListAsync(viewModel.BrandId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.AvailableTags = await LoadAvailableTagsAsync(); // Load all available tags
        viewModel.AvailableArticles = await LoadAvailableArticlesAsync(); // Load all available articles

        return View(viewModel);
    }

    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Bad request: ID mismatch during Product edit. Path ID: {PathId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        // Explicitly validate using FluentValidation
        var validationResult = await _validator.ValidateAsync(viewModel);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
        }

        // Check ModelState after FluentValidation results are added
        if (ModelState.IsValid)
        {
            Product? product = await _context.Set<Product>()
                                            .Include(p => p.Images) // Include images to manage changes
                                            .Include(p => p.ProductTags) // Include for managing relationships
                                            .Include(p => p.ArticleProducts) // Include for managing relationships
                                            .FirstOrDefaultAsync(p => p.Id == id); // Fetch tracked entity

            if (product == null)
            {
                _logger.LogWarning("Product not found for editing (POST): ID {Id}", id);
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            // Map updates from viewModel onto the tracked entity, EXCLUDING collections
            _mapper.Map(viewModel, product);

            // --- Handle Product Images ---
            var existingImages = product.Images.ToList(); // Keep track of original images
            var updatedImages = viewModel.Images ?? new List<ProductImageViewModel>();

            var imagesToKeep = new List<ProductImage>();

            foreach (var imgVm in updatedImages)
            {
                if (imgVm._Delete)
                {
                    // Image is marked for deletion
                    if (imgVm.Id > 0) // Only delete existing images
                    {
                        var existingImage = existingImages.FirstOrDefault(img => img.Id == imgVm.Id);
                        if (existingImage != null)
                        {
                            _context.Set<ProductImage>().Remove(existingImage);
                            _logger.LogDebug("Marked ProductImage {ImageId} for deletion from Product {ProductId}", imgVm.Id, product.Id);
                        }
                    }
                }
                else
                {
                    // Image is to be added or updated
                    if (imgVm.Id <= 0) // New image
                    {
                        var newImage = _mapper.Map<ProductImage>(imgVm);
                        newImage.ProductId = product.Id; // Ensure FK is set
                        product.Images.Add(newImage);
                        _logger.LogDebug("Added new ProductImage for Product {ProductId}", product.Id);
                        imagesToKeep.Add(newImage); // Add to keep list
                    }
                    else // Existing image
                    {
                        var existingImage = existingImages.FirstOrDefault(img => img.Id == imgVm.Id);
                        if (existingImage != null)
                        {
                            // Update properties of the existing image
                            _mapper.Map(imgVm, existingImage);
                            imagesToKeep.Add(existingImage); // Add to keep list
                            _logger.LogDebug("Updated ProductImage {ImageId} for Product {ProductId}", imgVm.Id, product.Id);
                        }
                        else
                        {
                            // This scenario shouldn't happen if VM comes from fetching, but handle defensively
                            _logger.LogWarning("Attempted to update ProductImage with ID {ImageId} not found in tracked entity for Product {ProductId}", imgVm.Id, product.Id);
                            // Decide behavior: ignore, log error, or add as new? Add as new might duplicate. Ignore for now.
                        }
                    }
                }
            }

            // Ensure images collection reflects changes (EF Change Tracker handles this, but explicit is clearer)
            product.Images = imagesToKeep;


            // --- Handle ProductTags ---
            var existingTagIds = product.ProductTags.Select(pt => pt.TagId).ToList();
            var selectedTagIds = viewModel.SelectedTagIds ?? new List<int>();

            var tagsToAdd = selectedTagIds.Except(existingTagIds).ToList();
            var tagsToRemove = existingTagIds.Except(selectedTagIds).ToList();

            // Add new ProductTags
            foreach (var tagId in tagsToAdd)
            {
                if (await _context.Set<domain.Entities.Tag>().AnyAsync(t => t.Id == tagId)) // Re-check existence
                {
                    product.ProductTags.Add(new ProductTag { ProductId = product.Id, TagId = tagId });
                }
            }

            // Remove old ProductTags
            var productTagsToRemove = product.ProductTags.Where(pt => tagsToRemove.Contains(pt.TagId)).ToList();
            foreach (var productTag in productTagsToRemove)
            {
                _context.Set<ProductTag>().Remove(productTag);
            }

            // --- Handle ArticleProducts (Products referencing Articles) ---
            var existingArticleIds = product.ArticleProducts.Select(ap => ap.ArticleId).ToList();
            var selectedArticleIds = viewModel.SelectedArticleIds ?? new List<int>();

            var articlesToAdd = selectedArticleIds.Except(existingArticleIds).ToList();
            var articlesToRemove = existingArticleIds.Except(selectedArticleIds).ToList();

            // Add new ArticleProducts
            foreach (var articleId in articlesToAdd)
            {
                if (await _context.Set<domain.Entities.Article>().AnyAsync(a => a.Id == articleId)) // Re-check existence
                {
                    product.ArticleProducts.Add(new ArticleProduct { ProductId = product.Id, ArticleId = articleId, OrderIndex = 0 }); // Add OrderIndex if needed
                }
            }

            // Remove old ArticleProducts
            var articleProductsToRemove = product.ArticleProducts.Where(ap => articlesToRemove.Contains(ap.ArticleId)).ToList();
            foreach (var articleProduct in articleProductsToRemove)
            {
                _context.Set<ArticleProduct>().Remove(articleProduct);
            }


            // Set UpdatedAt if not handled by DbContext interceptors
            // product.UpdatedAt = DateTime.UtcNow;


            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Product updated successfully: {Name} (ID: {Id})", product.Name, product.Id);
                TempData["SuccessMessage"] = $"Đã cập nhật sản phẩm '{product.Name}' thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating product: ID {Id}, Name: {Name}", id, viewModel.Name);
                if (ex.InnerException?.Message.Contains("idx_products_slug", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("products.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng bởi sản phẩm khác.");
                }
                else if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // This error can happen if a selected ID (Category, Brand, Tag, Article) was deleted by another user
                    ModelState.AddModelError("", "Lỗi liên kết dữ liệu. Vui lòng kiểm tra lại danh mục, thương hiệu, thẻ, hoặc bài viết liên quan.");
                }
                else if (ex.InnerException?.Message.Contains("idx_product_images_product_main", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // This error might indicate more than one IsMain image saved, though FluentValidation should catch it
                    ModelState.AddModelError("Images", "Chỉ được chọn một ảnh chính cho sản phẩm.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating product: ID {Id}, Name: {Name}", id, viewModel.Name);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật sản phẩm.");
            }

            // If validation or save failed, reload dropdowns/select lists before returning view
            viewModel.CategoryOptions = await LoadCategorySelectListAsync(viewModel.CategoryId);
            viewModel.BrandOptions = await LoadBrandSelectListAsync(viewModel.BrandId);
            viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
            viewModel.AvailableTags = await LoadAvailableTagsAsync();
            viewModel.AvailableArticles = await LoadAvailableArticlesAsync();
        }

        return View(viewModel);
    }


    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Product? product = await _context.Set<Product>()
                                           // No need to include related collections if cascade delete is configured
                                           .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product not found for deletion: ID {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }

        // No dependency check needed here if deleting the Product cascades to related tables (Images, ProductTag, ArticleProduct).
        // If Product is a dependency *for* other entities with non-cascading FKs, check here.

        try
        {
            string productName = product.Name;
            _context.Remove(product); // EF Core will handle cascade delete based on configuration
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product deleted successfully: {Name} (ID: {Id})", productName, id);
            return Json(new { success = true, message = $"Xóa sản phẩm '{productName}' thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting product (DbUpdateException): ID {Id}, Name: {Name}", id, product.Name);
            // Fallback for unexpected DB constraints
            return Json(new { success = false, message = "Đã xảy ra lỗi cơ sở dữ liệu khi xóa sản phẩm. Có thể sản phẩm vẫn còn được liên kết ở đâu đó." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product: ID {Id}, Name: {Name}", id, product.Name);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }


    // --- Helper Methods ---

    // Reusing from Article/Category
    private List<SelectListItem> GetStatusSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Đang kích hoạt", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Đã hủy kích hoạt", Selected = selectedValue == false }
        };
    }

    private List<SelectListItem> GetYesNoAllSelectList(bool? selectedValue)
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Tất cả", Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }

    // Reusing pattern from Article
    private List<SelectListItem> GetPublishStatusSelectList(PublishStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(PublishStatus))
           .Cast<PublishStatus>()
           .Select(t => new SelectListItem
           {
               Value = ((int)t).ToString(),
               Text = t.GetDisplayName(), // Assuming GetDisplayName Extension works for enums
               Selected = selectedStatus.HasValue && t == selectedStatus.Value
           })
           .OrderBy(t => t.Text)
           .ToList();

        // Add "All Statuses" option for filter
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


    private async Task<List<SelectListItem>> LoadCategorySelectListAsync(int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                          .Where(c => c.Type == CategoryType.Product && c.IsActive) // Filter by Product type
                          .OrderBy(c => c.OrderIndex)
                          .ThenBy(c => c.Name)
                          .AsNoTracking()
                          .Select(c => new { c.Id, c.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn danh mục (không bắt buộc) --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedValue.HasValue && c.Id == selectedValue.Value
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadBrandSelectListAsync(int? selectedValue = null)
    {
        var brands = await _context.Set<Brand>()
                          .Where(b => b.IsActive)
                          .OrderBy(b => b.Name)
                          .AsNoTracking()
                          .Select(b => new { b.Id, b.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn thương hiệu (không bắt buộc) --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(brands.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Name,
            Selected = selectedValue.HasValue && b.Id == selectedValue.Value
        }));

        return items;
    }


    private async Task<List<SelectListItem>> LoadAvailableTagsAsync(int? selectedValue = null)
    {
        var tags = await _context.Set<domain.Entities.Tag>()
                                 .OrderBy(t => t.Name)
                                 .AsNoTracking()
                                 .ToListAsync();

        var items = new List<SelectListItem>
        {
        };

        items.AddRange(tags.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Name,
            Selected = selectedValue.HasValue && b.Id == selectedValue.Value
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadAvailableArticlesAsync(int? selectedValue = null)
    {
        var articles = await _context.Set<domain.Entities.Article>()
                                 .Where(a => a.Status == PublishStatus.Published)
                                 .OrderByDescending(a => a.PublishedAt)
                                 .AsNoTracking()
                                 .ToListAsync();



        var items = new List<SelectListItem>
        {
        };

        items.AddRange(articles.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Title,
            Selected = selectedValue.HasValue && b.Id == selectedValue.Value
        }));

        return items;
    }
}