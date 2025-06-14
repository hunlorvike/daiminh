using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.Services.Interfaces;
using web.Areas.Client.ViewModels;
using X.PagedList.EF;

namespace web.Areas.Client.Services;

[Register(ServiceLifetime.Scoped)]
public class ProductClientService : IProductClientService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductClientService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductIndexViewModel> GetProductIndexViewModelAsync(ProductFilterViewModel filter, int pageNumber, int pageSize)
    {
        var productsQuery = _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive && p.Status == PublishStatus.Published);

        // Áp dụng bộ lọc
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var lowerTerm = filter.SearchTerm.ToLower();
            productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(lowerTerm) || (p.Brand != null && p.Brand.Name.ToLower().Contains(lowerTerm)));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId > 0)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.BrandId.HasValue && filter.BrandId > 0)
        {
            productsQuery = productsQuery.Where(p => p.BrandId == filter.BrandId.Value);
        }

        // Áp dụng sắp xếp
        productsQuery = filter.SortBy switch
        {
            "price_asc" => productsQuery.OrderBy(p => p.Id), // Cần thêm trường giá để sort
            "price_desc" => productsQuery.OrderByDescending(p => p.Id), // Cần thêm trường giá để sort
            "newest" => productsQuery.OrderByDescending(p => p.CreatedAt),
            _ => productsQuery.OrderByDescending(p => p.IsFeatured).ThenByDescending(p => p.CreatedAt) // Mặc định
        };

        var pagedProducts = await productsQuery
            .ProjectTo<ProductCardViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        // Chuẩn bị dữ liệu cho các dropdown của bộ lọc
        filter.Categories = await GetCategoryOptionsAsync(filter.CategoryId);
        filter.Brands = await GetBrandOptionsAsync(filter.BrandId);
        filter.SortOptions = GetSortOptions(filter.SortBy);

        return new ProductIndexViewModel
        {
            Products = pagedProducts,
            Filter = filter
        };
    }

    public async Task<ProductDetailViewModel?> GetProductDetailBySlugAsync(string slug)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Images)
                .ThenInclude(i => i) // Đảm bảo Images được load
            .Where(p => p.Slug == slug && p.IsActive && p.Status == PublishStatus.Published)
            .FirstOrDefaultAsync();

        if (product == null)
        {
            return null;
        }

        // Tăng lượt xem (không cần đợi kết quả)
        product.ViewCount++;
        _context.Products.Update(product);
        _context.Entry(product).Property(x => x.ViewCount).IsModified = true;
        // Bỏ qua các trường tracking khác để chỉ cập nhật ViewCount
        _context.Entry(product).Property(x => x.UpdatedAt).IsModified = false;
        _context.Entry(product).Property(x => x.UpdatedBy).IsModified = false;
        await _context.SaveChangesAsync();


        var viewModel = _mapper.Map<ProductDetailViewModel>(product);

        return viewModel;
    }
    private async Task<List<SelectListItem>> GetCategoryOptionsAsync(int? selectedId)
    {
        var items = await _context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive && c.Type == CategoryType.Product)
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToListAsync();

        items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả danh mục" });
        if (selectedId.HasValue)
        {
            var selectedItem = items.FirstOrDefault(x => x.Value == selectedId.Value.ToString());
            if (selectedItem != null) selectedItem.Selected = true;
        }
        return items;
    }

    private async Task<List<SelectListItem>> GetBrandOptionsAsync(int? selectedId)
    {
        var items = await _context.Brands
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
            .ToListAsync();

        items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả thương hiệu" });
        if (selectedId.HasValue)
        {
            var selectedItem = items.FirstOrDefault(x => x.Value == selectedId.Value.ToString());
            if (selectedItem != null) selectedItem.Selected = true;
        }
        return items;
    }

    private List<SelectListItem> GetSortOptions(string? selectedValue)
    {
        var options = new List<SelectListItem>
        {
            new SelectListItem { Value = "default", Text = "Sắp xếp mặc định" },
            new SelectListItem { Value = "newest", Text = "Mới nhất" },
            new SelectListItem { Value = "price_asc", Text = "Giá: Tăng dần" },
            new SelectListItem { Value = "price_desc", Text = "Giá: Giảm dần" }
        };

        var selected = options.FirstOrDefault(o => o.Value == selectedValue);
        if (selected != null)
        {
            selected.Selected = true;
        }
        else
        {
            options.First(o => o.Value == "default").Selected = true;
        }

        return options;
    }
}