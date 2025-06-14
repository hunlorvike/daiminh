using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.Services.Interfaces;
using web.Areas.Client.ViewModels;
using X.PagedList.EF;

namespace web.Areas.Client.Services;

[Register(ServiceLifetime.Scoped)]
public class BrandClientService : IBrandClientService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BrandClientService> _logger;

    public BrandClientService(ApplicationDbContext context, IMapper mapper, ILogger<BrandClientService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BrandDetailViewModel?> GetBrandDetailBySlugAsync(string slug, int pageNumber, int pageSize)
    {
        var brand = await _context.Brands
            .AsNoTracking()
            .Where(b => b.Slug == slug && b.IsActive)
            .FirstOrDefaultAsync();

        if (brand == null)
        {
            _logger.LogWarning("Không tìm thấy thương hiệu với slug: {Slug}", slug);
            return null; // Controller sẽ trả về 404 Not Found
        }

        var viewModel = _mapper.Map<BrandDetailViewModel>(brand);

        // Lấy danh sách sản phẩm của thương hiệu đó có phân trang
        var productsQuery = _context.Products
            .AsNoTracking()
            .Where(p => p.BrandId == brand.Id && p.IsActive && p.Status == PublishStatus.Published)
            .Include(p => p.Images) // Cần include để mapper có thể lấy ảnh
            .OrderByDescending(p => p.CreatedAt);

        var pagedProducts = await productsQuery
            .ProjectTo<ProductCardViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        viewModel.Products = pagedProducts;

        return viewModel;
    }
}