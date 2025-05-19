using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

public class BrandService : IBrandService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandService> _logger;

    public BrandService(ApplicationDbContext context, IMapper mapper, ILogger<BrandService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<BrandListItemViewModel>> GetPagedBrandsAsync(BrandFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Brand> query = _context.Set<Brand>()
                                    .Include(b => b.Products)
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(b => b.Name.ToLower().Contains(lowerSearchTerm) ||
                                     b.Description != null && b.Description.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(b => b.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(b => b.Name);

        IPagedList<BrandListItemViewModel> brandsPaged = await query
            .ProjectTo<BrandListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return brandsPaged;
    }

    public async Task<BrandViewModel?> GetBrandByIdAsync(int id)
    {
        Brand? brand = await _context.Set<Brand>()
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(b => b.Id == id);

        return _mapper.Map<BrandViewModel>(brand);
    }

    public async Task<OperationResult<int>> CreateBrandAsync(BrandViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        var brand = _mapper.Map<Brand>(viewModel);

        _context.Add(brand);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Brand: ID={Id}, Name={Name}", brand.Id, brand.Name);
            return OperationResult<int>.SuccessResult(brand.Id, $"Thêm thương hiệu '{brand.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo thương hiệu: {Name}", viewModel.Name);
            return OperationResult<int>.FailureResult(message: "Lỗi cơ sở dữ liệu khi lưu thương hiệu.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi lưu thương hiệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo thương hiệu: {Name}", viewModel.Name);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu thương hiệu.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu thương hiệu." });
        }
    }

    public async Task<OperationResult> UpdateBrandAsync(BrandViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        var brand = await _context.Set<Brand>().FirstOrDefaultAsync(b => b.Id == viewModel.Id);
        if (brand == null)
        {
            _logger.LogWarning("Brand not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy thương hiệu để cập nhật.");
        }

        _mapper.Map(viewModel, brand);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Brand: ID={Id}, Name={Name}", brand.Id, brand.Name);
            return OperationResult.SuccessResult($"Cập nhật thương hiệu '{brand.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật thương hiệu ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Lỗi cơ sở dữ liệu khi cập nhật thương hiệu.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi cập nhật thương hiệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật thương hiệu ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật thương hiệu.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật thương hiệu." });
        }
    }

    public async Task<OperationResult> DeleteBrandAsync(int id)
    {
        var brand = await _context.Set<Brand>()
                                  .Include(b => b.Products)
                                  .FirstOrDefaultAsync(b => b.Id == id);

        if (brand == null)
        {
            _logger.LogWarning("Brand not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy thương hiệu.");
        }

        if (brand.Products?.Any() == true)
        {
            _logger.LogWarning("Cannot delete Brand {Name} (ID: {Id}) due to related products.", brand.Name, id);
            return OperationResult.FailureResult($"Không thể xóa thương hiệu '{brand.Name}' vì đang được sử dụng bởi {brand.Products.Count} sản phẩm.");
        }

        _context.Remove(brand);

        try
        {
            string brandName = brand.Name;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Brand: ID={Id}, Name={Name}", id, brandName);
            return OperationResult.SuccessResult($"Xóa thương hiệu '{brandName}' thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa thương hiệu ID: {Id}", id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi xóa thương hiệu.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa thương hiệu." });
        }
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

    // **Service handles DB-related validation logic**
    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false; // Slug cannot be empty for uniqueness check

        var query = _context.Set<Brand>()
                            .Where(b => b.Slug.ToLower() == slug.Trim().ToLower());

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(b => b.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    // **Service handles business logic check**
    public async Task<bool> HasRelatedProductsAsync(int brandId)
    {
        return await _context.Set<Product>().AnyAsync(p => p.BrandId == brandId);
    }
}