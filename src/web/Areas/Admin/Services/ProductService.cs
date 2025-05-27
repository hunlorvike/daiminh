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

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                                     (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Manufacturer != null && p.Manufacturer.ToLower().Contains(lowerSearchTerm)));
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
                                     .Include(p => p.ProductTags)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        ProductViewModel viewModel = _mapper.Map<ProductViewModel>(product);
        await PopulateProductViewModelSelectListsAsync(viewModel);
        return viewModel;
    }

    public async Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult("Slug này đã tồn tại.");
        }

        var product = _mapper.Map<Product>(viewModel);
        UpdateProductRelationships(product, viewModel.SelectedTagIds);
        UpdateProductImages(product, viewModel.Images);

        _context.Add(product);

        try
        {
            await _context.Database.BeginTransactionAsync();
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            _logger.LogInformation("Đã tạo Sản phẩm: ID={Id}, Name={Name}", product.Id, product.Name);
            return OperationResult<int>.SuccessResult(product.Id, $"Đã thêm sản phẩm '{product.Name}' thành công.");
        }
        catch (Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            _logger.LogError(ex, "Lỗi khi tạo Sản phẩm: {Name}", viewModel.Name);
            if (ex.InnerException?.Message.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult("Slug này đã tồn tại.");
            }
            return OperationResult<int>.FailureResult($"Không thể thêm sản phẩm '{viewModel.Name}'. Lỗi: {ex.Message}");
        }
    }

    public async Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult("Slug này đã tồn tại.");
        }

        var product = await _context.Set<Product>()
            .Include(p => p.Images)
            .Include(p => p.ProductTags)
            .FirstOrDefaultAsync(p => p.Id == viewModel.Id);

        if (product == null)
        {
            return OperationResult.FailureResult("Không tìm thấy sản phẩm.");
        }

        _mapper.Map(viewModel, product);
        UpdateProductRelationships(product, viewModel.SelectedTagIds);
        UpdateProductImages(product, viewModel.Images);

        try
        {
            await _context.Database.BeginTransactionAsync();
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            _logger.LogInformation("Đã cập nhật Sản phẩm: ID={Id}, Name={Name}", product.Id, product.Name);
            return OperationResult.SuccessResult($"Đã cập nhật sản phẩm '{product.Name}' thành công.");
        }
        catch (Exception ex)
        {
            await _context.Database.RollbackTransactionAsync();
            _logger.LogError(ex, "Lỗi khi cập nhật Sản phẩm ID: {Id}", viewModel.Id);
            if (ex.InnerException?.Message.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Slug này đã tồn tại.");
            }
            return OperationResult.FailureResult($"Không thể cập nhật sản phẩm '{viewModel.Name}'. Lỗi: {ex.Message}");
        }
    }

    public async Task<OperationResult> DeleteProductAsync(int id)
    {
        var product = await _context.Set<Product>().FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return OperationResult.FailureResult("Không tìm thấy sản phẩm.");
        }

        string name = product.Name;
        _context.Remove(product);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Đã xóa Sản phẩm: ID={Id}, Name={Name}", id, name);
            return OperationResult.SuccessResult($"Xóa sản phẩm '{name}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa Sản phẩm ID: {Id}", id);
            return OperationResult.FailureResult($"Không thể xóa sản phẩm '{name}'. Lỗi: {ex.Message}");
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;
        var query = _context.Set<Product>().Where(p => p.Slug == slug);
        if (ignoreId.HasValue)
        {
            query = query.Where(p => p.Id != ignoreId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task<List<SelectListItem>> GetProductCategorySelectListAsync(int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                          .Where(c => c.Type == CategoryType.Product && c.IsActive)
                          .OrderBy(c => c.OrderIndex).ThenBy(c => c.Name)
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

    public async Task<List<SelectListItem>> GetBrandSelectListAsync(int? selectedValue = null)
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

    public async Task<List<SelectListItem>> GetTagSelectListAsync(List<int>? selectedValues = null)
    {
        var tags = await _context.Set<Tag>()
                          .Where(t => t.Type == TagType.Product)
                          .OrderBy(t => t.Name)
                          .AsNoTracking()
                          .Select(t => new { t.Id, t.Name })
                          .ToListAsync();
        var items = new List<SelectListItem>();
        items.AddRange(tags.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));
        return items;
    }

    public async Task PopulateProductViewModelSelectListsAsync(ProductViewModel viewModel)
    {
        viewModel.CategoryOptions = await GetProductCategorySelectListAsync(viewModel.CategoryId);
        viewModel.BrandOptions = await GetBrandSelectListAsync(viewModel.BrandId);
        viewModel.TagOptions = await GetTagSelectListAsync(viewModel.SelectedTagIds);
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


    private void UpdateProductRelationships(Product product, List<int>? selectedTagIds)
    {
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
    }

    private void UpdateProductImages(Product product, List<ProductImageViewModel>? imageViewModels)
    {
        var existingImages = product.Images?.ToList() ?? new List<ProductImage>();
        var updatedImageViewModels = imageViewModels?.Where(img => !img.IsDeleted).ToList() ?? new List<ProductImageViewModel>();

        var imageIdsToKeepOrAdd = updatedImageViewModels.Where(img => img.Id > 0).Select(img => img.Id).ToHashSet();
        foreach (var existingImage in existingImages)
        {
            if (existingImage.Id > 0 && !imageIdsToKeepOrAdd.Contains(existingImage.Id))
            {
                _context.Remove(existingImage);
            }
        }

        var newOrUpdatedImages = new List<ProductImage>();
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
                    newOrUpdatedImages.Add(image);
                }
            }
            else
            {
                image = _mapper.Map<ProductImage>(imgVm);
                image.ProductId = product.Id;
                image.OrderIndex = i;
                _context.Add(image);
                newOrUpdatedImages.Add(image);
            }
        }
        product.Images = newOrUpdatedImages;
    }
}