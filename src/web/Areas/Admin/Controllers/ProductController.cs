using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.Validators.Product;
using web.Areas.Admin.ViewModels.Product;
using web.Areas.Admin.ViewModels.ProductVariation;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class ProductController : Controller
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
    public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ProductFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<Product> query = _context.Set<Product>()
                                             .Include(p => p.Category)
                                             .Include(p => p.Brand)
                                             .Include(p => p.Images)
                                             .Include(p => p.ProductTags)
                                             .Include(p => p.Variations)
                                             .Include(p => p.Reviews)
                                             .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm) ||
                                     (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == filter.BrandId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);
        }

        query = query
            .OrderByDescending(p => p.ViewCount)
            .ThenByDescending(p => p.CreatedAt);

        var productsPaged = await query.ProjectTo<ProductListItemViewModel>(_mapper.ConfigurationProvider)
                                       .ToPagedListAsync(pageNumber, currentPageSize);

        filter.CategoryOptions = await LoadCategorySelectListAsync(CategoryType.Product, filter.CategoryId);
        filter.BrandOptions = await LoadBrandSelectListAsync(filter.BrandId);
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
    public async Task<IActionResult> Create()
    {
        ProductViewModel viewModel = new()
        {
            IsFeatured = false,
            IsActive = true,
            Status = PublishStatus.Draft,
            SitemapPriority = 0.5,
            SitemapChangeFrequency = "monthly",
            Images = new List<ProductImageViewModel>()
        };

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel viewModel)
    {
        var result = await new ProductViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var product = _mapper.Map<Product>(viewModel);

        UpdateProductRelationships(product, viewModel.SelectedAttributeIds, viewModel.SelectedTagIds, viewModel.SelectedArticleIds);
        UpdateProductImages(product, viewModel.Images);

        _context.Add(product);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm sản phẩm '{product.Name}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi thêm sản phẩm: {Name}", viewModel.Name);
            ModelState.AddModelError("", "Slug có thể đã tồn tại hoặc dữ liệu không hợp lệ.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi thêm sản phẩm.");
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi lưu sản phẩm.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Product? product = await _context.Set<Product>()
                                         .Include(p => p.Category)
                                         .Include(p => p.Brand)
                                         .Include(p => p.Images)
                                         .Include(p => p.ProductAttributes)
                                         .Include(p => p.ProductTags)
                                         .Include(p => p.ArticleProducts)
                                         .Include(p => p.Variations)
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        ProductViewModel viewModel = _mapper.Map<ProductViewModel>(product);

        viewModel.VariationFilter = new ProductVariationFilterViewModel();

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("ID không khớp khi cập nhật sản phẩm. URL: {Id}, ViewModel: {ModelId}", id, viewModel.Id);
            return BadRequest();
        }

        var result = await new ProductViewModelValidator(_context).ValidateAsync(viewModel);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var product = await _context.Set<Product>()
            .Include(p => p.Images)
            .Include(p => p.ProductAttributes)
            .Include(p => p.ProductTags)
            .Include(p => p.ArticleProducts)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy sản phẩm.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, product);
        UpdateProductRelationships(product, viewModel.SelectedAttributeIds, viewModel.SelectedTagIds, viewModel.SelectedArticleIds);
        UpdateProductImages(product, viewModel.Images);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật sản phẩm '{product.Name}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật sản phẩm ID: {Id}", id);
            ModelState.AddModelError("", "Slug có thể đã tồn tại hoặc dữ liệu không hợp lệ.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi hệ thống khi cập nhật sản phẩm ID: {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Set<Product>().FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Không tìm thấy sản phẩm để xóa. ID: {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
        }

        try
        {
            string name = product.Name;
            _context.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Đã xóa sản phẩm '{Name}' (ID: {Id})", name, id);
            return Json(new { success = true, message = $"Xóa sản phẩm '{name}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa sản phẩm ID: {Id}", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }
}

public partial class ProductController
{
    private async Task PopulateViewModelSelectListsAsync(ProductViewModel viewModel)
    {
        viewModel.CategoryOptions = await LoadCategorySelectListAsync(CategoryType.Product, viewModel.CategoryId);
        viewModel.BrandOptions = await LoadBrandSelectListAsync(viewModel.BrandId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.AttributeOptions = await LoadAttributeSelectListAsync(viewModel.SelectedAttributeIds); // For ProductAttributes
        viewModel.TagOptions = await LoadTagSelectListAsync(TagType.Product, viewModel.SelectedTagIds);
        viewModel.ArticleOptions = await LoadArticleSelectListAsync(viewModel.SelectedArticleIds); // For ArticleProducts
    }

    private async Task<List<SelectListItem>> LoadCategorySelectListAsync(CategoryType categoryType, int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                          .Where(c => c.Type == categoryType && c.IsActive)
                          .OrderBy(c => c.OrderIndex)
                          .ThenBy(c => c.Name)
                          .AsNoTracking()
                          .Select(c => new { c.Id, c.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn danh mục --", Selected = !selectedValue.HasValue }
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
             new SelectListItem { Value = "", Text = "-- Chọn thương hiệu --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(brands.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Name,
            Selected = selectedValue.HasValue && b.Id == selectedValue.Value
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadAttributeSelectListAsync(List<int>? selectedValues = null)
    {
        var attributes = await _context.Set<domain.Entities.Attribute>()
                         .OrderBy(a => a.Name)
                         .AsNoTracking()
                         .Select(a => new { a.Id, a.Name })
                         .ToListAsync();

        var items = new List<SelectListItem>
        {
        };

        items.AddRange(attributes.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Name,
            Selected = selectedValues != null && selectedValues.Contains(a.Id)
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadTagSelectListAsync(TagType tagType, List<int>? selectedValues = null)
    {
        var tags = await _context.Set<Tag>()
                          .Where(t => t.Type == tagType)
                          .OrderBy(t => t.Name)
                          .AsNoTracking()
                          .Select(t => new { t.Id, t.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
        };

        items.AddRange(tags.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadArticleSelectListAsync(List<int>? selectedValues = null)
    {
        var articles = await _context.Set<Article>()
                          .Where(a => a.Status == PublishStatus.Published)
                          .OrderBy(a => a.Title)
                          .AsNoTracking()
                          .Select(a => new { a.Id, a.Title })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
        };

        items.AddRange(articles.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Title,
            Selected = selectedValues != null && selectedValues.Contains(a.Id)
        }));

        return items;
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

    private void UpdateProductRelationships(
        Product product,
        List<int>? selectedAttributeIds,
        List<int>? selectedTagIds,
        List<int>? selectedArticleIds)
    {
        var existingAttributeIds = product.ProductAttributes?.Select(pa => pa.AttributeId).ToList() ?? new List<int>();
        var attributeIdsToAdd = selectedAttributeIds?.Except(existingAttributeIds).ToList() ?? new List<int>();
        var attributeIdsToRemove = existingAttributeIds.Except(selectedAttributeIds ?? new List<int>()).ToList();

        foreach (var attributeId in attributeIdsToRemove)
        {
            var productAttribute = product.ProductAttributes!.First(pa => pa.AttributeId == attributeId);
            _context.Remove(productAttribute);
        }

        foreach (var attributeId in attributeIdsToAdd)
        {
            product.ProductAttributes ??= new List<ProductAttribute>();
            product.ProductAttributes.Add(new ProductAttribute { ProductId = product.Id, AttributeId = attributeId });
        }

        var existingTagIds = product.ProductTags?.Select(pt => pt.TagId).ToList() ?? new List<int>();
        var tagIdsToAdd = selectedTagIds?.Except(existingTagIds).ToList() ?? new List<int>();
        var tagIdsToRemove = existingTagIds.Except(selectedTagIds ?? new List<int>()).ToList();

        foreach (var tagId in tagIdsToRemove)
        {
            var productTag = product.ProductTags!.First(pt => pt.TagId == tagId);
            _context.Remove(productTag);
        }

        foreach (var tagId in tagIdsToAdd)
        {
            product.ProductTags ??= new List<ProductTag>();
            product.ProductTags.Add(new ProductTag { ProductId = product.Id, TagId = tagId });
        }

        var existingArticleIds = product.ArticleProducts?.Select(ap => ap.ArticleId).ToList() ?? new List<int>();
        var articleIdsToAdd = selectedArticleIds?.Except(existingArticleIds).ToList() ?? new List<int>();
        var articleIdsToRemove = existingArticleIds.Except(selectedArticleIds ?? new List<int>()).ToList();

        foreach (var articleId in articleIdsToRemove)
        {
            var articleProduct = product.ArticleProducts!.First(ap => ap.ArticleId == articleId);
            _context.Remove(articleProduct);
        }

        foreach (var articleId in articleIdsToAdd)
        {
            product.ArticleProducts ??= new List<ArticleProduct>();
            product.ArticleProducts.Add(new ArticleProduct { ProductId = product.Id, ArticleId = articleId });
        }
    }

    private void UpdateProductImages(Product product, List<ProductImageViewModel> imageViewModels)
    {
        var existingImages = product.Images ?? new List<ProductImage>();
        var updatedImageViewModels = imageViewModels?.Where(img => !img.IsDeleted).ToList() ?? new List<ProductImageViewModel>();

        var imageIdsToKeep = updatedImageViewModels.Where(img => img.Id > 0).Select(img => img.Id).ToHashSet();

        foreach (var existingImage in existingImages.ToList())
        {
            if (!imageIdsToKeep.Contains(existingImage.Id))
            {
                _context.Remove(existingImage);
                existingImages.Remove(existingImage);
            }
        }

        var updatedImageList = new List<ProductImage>();
        for (int i = 0; i < updatedImageViewModels.Count; i++)
        {
            var imgVm = updatedImageViewModels[i];
            ProductImage? image;

            if (imgVm.Id > 0)
            {
                image = existingImages.FirstOrDefault(img => img.Id == imgVm.Id);
                if (image != null)
                {
                    _mapper.Map(imgVm, image);
                    image.OrderIndex = i;
                    _context.Entry(image).State = EntityState.Modified;
                    updatedImageList.Add(image);
                }
            }
            else
            {
                image = _mapper.Map<ProductImage>(imgVm);
                image.ProductId = product.Id;
                image.OrderIndex = i;
                _context.Add(image);
                updatedImageList.Add(image);
            }
        }

        product.Images = updatedImageList;
    }
}