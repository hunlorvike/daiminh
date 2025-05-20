using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IAttributeService _attributeService;
    private readonly ITagService _tagService;

    public ProductService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductService> logger,
        ICategoryService categoryService,
        IBrandService brandService,
        IAttributeService attributeService,
        ITagService tagService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _categoryService = categoryService;
        _brandService = brandService;
        _attributeService = attributeService;
        _tagService = tagService;
    }

    public async Task<IPagedList<ProductListItemViewModel>> GetPagedProductsAsync(ProductFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Product> query = _context.Set<Product>()
                                        .Include(p => p.Category)
                                        .Include(p => p.Brand)
                                        .Include(p => p.Images!.Where(img => img.OrderIndex == 0))
                                        .AsNoTracking();


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm) ||
                                     p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm) ||
                                     p.Description != null && p.Description.ToLower().Contains(lowerSearchTerm) || // Search description might be slow, consider full-text search later
                                     p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.BrandId.HasValue && filter.BrandId.Value > 0)
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
                                   .ToPagedListAsync(pageNumber, pageSize);

        return productsPaged;
    }

    public async Task<ProductViewModel?> GetProductByIdAsync(int id)
    {
        Product? product = await _context.Set<Product>()
                                     .Include(p => p.Category)
                                     .Include(p => p.Brand)
                                     .Include(p => p.Images!.OrderBy(img => img.OrderIndex))
                                     .Include(p => p.ProductAttributes)
                                     .Include(p => p.ProductTags)
                                     .Include(p => p.ArticleProducts)
                                     .Include(p => p.Variations)
                                     .Include(p => p.Reviews)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        ProductViewModel viewModel = _mapper.Map<ProductViewModel>(product);

        await PopulateViewModelSelectListsInternalAsync(viewModel);

        viewModel.VariationFilter ??= new ProductVariationFilterViewModel();


        return viewModel;
    }

    public async Task<ProductViewModel> GetProductViewModelForCreateAsync()
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

        await PopulateViewModelSelectListsInternalAsync(viewModel);

        return viewModel;
    }


    public async Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel, string? authorId, string? authorName)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại.", errors: new List<string> { "Slug này đã tồn tại." });
        }

        if (viewModel.CategoryId.HasValue)
        {
            var category = await _categoryService.GetCategoryByIdAsync(viewModel.CategoryId.Value);
            if (category == null)
            {
                return OperationResult<int>.FailureResult(message: "Danh mục sản phẩm không tồn tại.", errors: new List<string> { "Danh mục sản phẩm không tồn tại." });
            }
        }
        if (viewModel.BrandId.HasValue)
        {
            var brand = await _brandService.GetBrandByIdAsync(viewModel.BrandId.Value);
            if (brand == null)
            {
                return OperationResult<int>.FailureResult(message: "Thương hiệu không tồn tại.", errors: new List<string> { "Thương hiệu không tồn tại." });
            }
        }


        var product = _mapper.Map<Product>(viewModel);

        UpdateProductRelationshipsInternal(product, viewModel.SelectedAttributeIds, viewModel.SelectedTagIds, viewModel.SelectedArticleIds);
        UpdateProductImagesInternal(product, viewModel.Images);

        _context.Add(product);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Product: ID={Id}, Name={Name}, Slug={Slug}", product.Id, product.Name, product.Slug);
            return OperationResult<int>.SuccessResult(product.Id, $"Đã thêm sản phẩm '{product.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi thêm sản phẩm: {Name}, Slug: {Slug}", viewModel.Name, viewModel.Slug);
            if (ex.InnerException?.Message?.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại.", errors: new List<string> { "Slug này đã tồn tại." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu sản phẩm.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu sản phẩm." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi thêm sản phẩm.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi lưu sản phẩm.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi lưu sản phẩm." });
        }
    }

    public async Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug có thể đã tồn tại.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        if (viewModel.CategoryId.HasValue)
        {
            var category = await _categoryService.GetCategoryByIdAsync(viewModel.CategoryId.Value);
            if (category == null)
            {
                return OperationResult.FailureResult(message: "Danh mục sản phẩm không tồn tại.", errors: new List<string> { "Danh mục sản phẩm không tồn tại." });
            }
        }
        if (viewModel.BrandId.HasValue)
        {
            var brand = await _brandService.GetBrandByIdAsync(viewModel.BrandId.Value);
            if (brand == null)
            {
                return OperationResult.FailureResult(message: "Thương hiệu không tồn tại.", errors: new List<string> { "Thương hiệu không tồn tại." });
            }
        }

        var product = await _context.Set<Product>()
            .Include(p => p.Images)
            .Include(p => p.ProductAttributes)
            .Include(p => p.ProductTags)
            .Include(p => p.ArticleProducts)
            .FirstOrDefaultAsync(p => p.Id == viewModel.Id);


        if (product == null)
        {
            _logger.LogWarning("Product not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy sản phẩm.");
        }

        _mapper.Map(viewModel, product);

        UpdateProductRelationshipsInternal(product, viewModel.SelectedAttributeIds, viewModel.SelectedTagIds, viewModel.SelectedArticleIds);
        UpdateProductImagesInternal(product, viewModel.Images);


        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Product: ID={Id}, Name={Name}, Slug={Slug}", product.Id, product.Name, product.Slug);
            return OperationResult.SuccessResult($"Đã cập nhật sản phẩm '{product.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật sản phẩm ID: {Id}, Name: {Name}", viewModel.Id, viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Slug có thể đã tồn tại.", errors: new List<string> { "Slug này đã được sử dụng." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật sản phẩm.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật sản phẩm." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật sản phẩm ID: {Id}, Name: {Name}", viewModel.Id, viewModel.Name);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn." });
        }
    }

    public async Task<OperationResult> DeleteProductAsync(int id)
    {
        var product = await _context.Set<Product>().FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            _logger.LogWarning("Product not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy sản phẩm.");
        }

        string name = product.Name;

        _context.Remove(product);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Product: ID={Id}, Name={Name}", id, name);
            return OperationResult.SuccessResult($"Xóa sản phẩm '{name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa sản phẩm ID: {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult($"Không thể xóa sản phẩm '{name}' vì đang được sử dụng.", errors: new List<string> { $"Không thể xóa sản phẩm '{name}' vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa sản phẩm.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa sản phẩm." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa sản phẩm ID: {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa sản phẩm.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa sản phẩm." });
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<Product>()
                            .Where(p => p.Slug.ToLower() == lowerSlug);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(p => p.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<List<SelectListItem>> GetProductSelectListAsync(int? selectedValue = null)
    {
        var products = await _context.Set<Product>()
                     .OrderBy(p => p.Name)
                     .AsNoTracking()
                     .Select(p => new { p.Id, p.Name })
                     .ToListAsync();

        var items = new List<SelectListItem>
        {
             new SelectListItem { Value = "", Text = "-- Tất cả sản phẩm --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name,
            Selected = selectedValue.HasValue && p.Id == selectedValue.Value
        }));

        return items;
    }

    private async Task PopulateViewModelSelectListsInternalAsync(ProductViewModel viewModel)
    {
        viewModel.CategoryOptions = await _categoryService.GetParentCategorySelectListAsync(CategoryType.Product, viewModel.CategoryId);
        viewModel.BrandOptions = await _brandService.GetBrandSelectListAsync(viewModel.BrandId);
        viewModel.AttributeOptions = await _attributeService.GetAttributeSelectListAsync(viewModel.SelectedAttributeIds);
        viewModel.TagOptions = await _tagService.GetTagSelectListAsync(TagType.Product, viewModel.SelectedTagIds);
    }

    private void UpdateProductRelationshipsInternal(
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
            var productAttribute = product.ProductAttributes?.FirstOrDefault(at => at.AttributeId == attributeId);
            if (productAttribute != null) _context.Remove(productAttribute);
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
            var productTag = product.ProductTags?.FirstOrDefault(pt => pt.TagId == tagId);
            if (productTag != null) _context.Remove(productTag);
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
            var articleProduct = product.ArticleProducts?.FirstOrDefault(ap => ap.ArticleId == articleId);
            if (articleProduct != null) _context.Remove(articleProduct);
        }

        foreach (var articleId in articleIdsToAdd)
        {
            product.ArticleProducts ??= new List<ArticleProduct>();
            product.ArticleProducts.Add(new ArticleProduct { ProductId = product.Id, ArticleId = articleId });
        }
    }


    private void UpdateProductImagesInternal(Product product, List<ProductImageViewModel>? imageViewModels)
    {
        var existingImages = product.Images ?? new List<ProductImage>();
        var updatedImageViewModels = imageViewModels?.Where(img => !img.IsDeleted).ToList() ?? new List<ProductImageViewModel>();

        var imageIdsToKeepOrAdd = updatedImageViewModels.Where(img => img.Id > 0).Select(img => img.Id).ToHashSet();
        foreach (var existingImage in existingImages.ToList())
        {
            if (existingImage.Id > 0 && !imageIdsToKeepOrAdd.Contains(existingImage.Id))
            {
                _context.Remove(existingImage);
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
                else
                {
                    _logger.LogWarning("Existing image ID {ImageId} from ViewModel not found in loaded entity images for product {ProductId}", imgVm.Id, product.Id);
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

    public async Task<List<SelectListItem>> GetProductSelectListAsync(List<int>? selectedValues = null)
    {
        var products = await _context.Set<Product>()
                     .OrderBy(t => t.Name)
                     .AsNoTracking()
                     .Select(t => new { t.Id, t.Name })
                     .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn sản phẩm --", Selected = selectedValues == null || !selectedValues.Any() }
        };

        items.AddRange(products.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));

        return items;
    }
}