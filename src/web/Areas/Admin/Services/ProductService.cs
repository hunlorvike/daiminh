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
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<ProductListItemViewModel>> GetPagedProductsAsync(ProductFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Product> query = _context.Set<Product>()
                                         .Include(p => p.Category)
                                         .Include(p => p.Brand)
                                         .Include(p => p.Images)
                                         .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(lowerSearchTerm) ||
                                     (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Brand != null && p.Brand.Name.ToLower().Contains(lowerSearchTerm)) ||
                                     (p.Category != null && p.Category.Name.ToLower().Contains(lowerSearchTerm)));
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
        if (filter.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == filter.IsActive.Value);
        }
        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);
        }

        query = query.OrderByDescending(p => p.UpdatedAt).ThenBy(p => p.Name);

        return await query
            .ProjectTo<ProductListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);
    }

    public async Task<ProductViewModel?> GetProductByIdAsync(int id)
    {
        Product? product = await _context.Set<Product>()
                                     .Include(p => p.Category)
                                     .Include(p => p.Brand)
                                     .Include(p => p.Images!.OrderBy(i => i.OrderIndex))
                                     .Include(p => p.ProductTags!)
                                        .ThenInclude(pt => pt.Tag)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;
        return _mapper.Map<ProductViewModel>(product);
    }

    public async Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug sản phẩm này đã tồn tại.", errors: new List<string> { "Slug sản phẩm này đã tồn tại." });
        }

        var executionStrategy = _context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = _mapper.Map<Product>(viewModel);

                product.Images = _mapper.Map<List<ProductImage>>(viewModel.Images);
                EnsureOneMainImage(product.Images);

                UpdateProductTags(product, viewModel.SelectedTagIds);

                _context.Add(product);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Created Product: ID={Id}, Name={Name}, Slug={Slug}", product.Id, product.Name, product.Slug);
                return OperationResult<int>.SuccessResult(product.Id, $"Thêm sản phẩm '{product.Name}' thành công.");
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi DB khi tạo sản phẩm: {Name}", viewModel.Name);
                if (ex.InnerException?.Message?.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return OperationResult<int>.FailureResult(message: "Slug sản phẩm này đã tồn tại.", errors: new List<string> { "Slug sản phẩm này đã tồn tại." });
                }
                return OperationResult<int>.FailureResult(message: "Lỗi cơ sở dữ liệu khi lưu sản phẩm.", errors: new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi không xác định khi tạo sản phẩm.");
                return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi lưu sản phẩm.", errors: new List<string> { ex.Message });
            }
        });
    }

    public async Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug sản phẩm này đã tồn tại.", errors: new List<string> { "Slug sản phẩm này đã tồn tại." });
        }

        var executionStrategy = _context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Set<Product>()
                    .Include(p => p.Images)
                    .Include(p => p.ProductTags)
                    .FirstOrDefaultAsync(p => p.Id == viewModel.Id);

                if (product == null)
                {
                    _logger.LogWarning("Product not found for update. ID: {Id}", viewModel.Id);
                    return OperationResult.FailureResult("Không tìm thấy sản phẩm để cập nhật.");
                }

                _mapper.Map(viewModel, product);

                UpdateProductImages(product, viewModel.Images);
                EnsureOneMainImage(product.Images);
                UpdateProductTags(product, viewModel.SelectedTagIds);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Updated Product: ID={Id}, Name={Name}, Slug={Slug}", product.Id, product.Name, product.Slug);
                return OperationResult.SuccessResult($"Cập nhật sản phẩm '{product.Name}' thành công.");
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi DB khi cập nhật sản phẩm ID: {Id}", viewModel.Id);
                if (ex.InnerException?.Message?.Contains("UQ_Product_Slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return OperationResult.FailureResult(message: "Slug sản phẩm này đã tồn tại.", errors: new List<string> { "Slug sản phẩm này đã tồn tại." });
                }
                return OperationResult.FailureResult(message: "Lỗi cơ sở dữ liệu khi cập nhật sản phẩm.", errors: new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật sản phẩm ID: {Id}", viewModel.Id);
                return OperationResult.FailureResult(message: "Lỗi hệ thống khi cập nhật sản phẩm.", errors: new List<string> { ex.Message });
            }
        });
    }

    public async Task<OperationResult> DeleteProductAsync(int id)
    {
        var product = await _context.Set<Product>()
                                .Include(p => p.Images)
                                .Include(p => p.ProductTags)
                                .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy sản phẩm.");
        }
        if (product.Images != null) _context.RemoveRange(product.Images);
        if (product.ProductTags != null) _context.RemoveRange(product.ProductTags);

        _context.Remove(product);
        string productName = product.Name;
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Product: ID={Id}, Name={Name}", id, productName);
            return OperationResult.SuccessResult($"Xóa sản phẩm '{productName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa sản phẩm ID {Id}", id);
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa sản phẩm.", errors: new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa sản phẩm ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa sản phẩm.", errors: new List<string> { ex.Message });
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

    public async Task<List<SelectListItem>> GetProductSelectListAsync(List<int>? selectedValue = null)
    {
        var products = await _context.Set<Product>()
            .Where(a => a.Status == PublishStatus.Published && a.IsActive)
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .Select(a => new { a.Id, a.Name })
            .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn sản phẩm --", Selected = selectedValue == null || !selectedValue.Any() }
        };
        items.AddRange(products.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValue != null && selectedValue.Contains(t.Id)
        }));
        return items;
    }

    private void UpdateProductTags(Product product, List<int>? selectedTagIds)
    {
        product.ProductTags ??= new List<ProductTag>();
        var currentTagIds = product.ProductTags.Select(pt => pt.TagId).ToList();

        var tagsToRemove = product.ProductTags.Where(pt => !(selectedTagIds?.Contains(pt.TagId) ?? false)).ToList();
        foreach (var tagToRemove in tagsToRemove)
        {
            product.ProductTags.Remove(tagToRemove);
        }

        if (selectedTagIds != null)
        {
            var tagIdsToAdd = selectedTagIds.Except(currentTagIds).ToList();
            foreach (var tagIdToAdd in tagIdsToAdd)
            {
                product.ProductTags.Add(new ProductTag { TagId = tagIdToAdd });
            }
        }
    }

    private void UpdateProductImages(Product product, List<ProductImageViewModel> imageVMs)
    {
        var imagesToRemove = product.Images!.Where(img => !imageVMs.Any(vm => vm.Id == img.Id && vm.Id != 0)).ToList();
        foreach (var imgToRemove in imagesToRemove)
        {
            _context.Remove(imgToRemove);
            product.Images!.Remove(imgToRemove);
        }

        foreach (var imgVM in imageVMs)
        {
            if (imgVM.Id > 0)
            {
                var existingImage = product.Images!.FirstOrDefault(img => img.Id == imgVM.Id);
                if (existingImage != null)
                {
                    _mapper.Map(imgVM, existingImage);
                }
            }
            else
            {
                var newImage = _mapper.Map<ProductImage>(imgVM);
                product.Images!.Add(newImage);
            }
        }
        EnsureOneMainImage(product.Images);
    }

    private void EnsureOneMainImage(ICollection<ProductImage>? images)
    {
        if (images == null || images.Count == 0) return;

        var mainImages = images.Where(i => i.IsMain).ToList();
        if (mainImages.Count > 1)
        {
            for (int i = 1; i < mainImages.Count; i++)
            {
                mainImages[i].IsMain = false;
            }
        }
        else if (mainImages.Count == 0)
        {
            images.First().IsMain = true;
        }
    }
}