using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Client.Services.Interfaces;
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Services;

[Register(ServiceLifetime.Scoped)]
public class HomeService : IHomeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HomeService> _logger;

    public HomeService(ApplicationDbContext context, IMapper mapper, ILogger<HomeService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<HomeViewModel> GetHomeViewModelAsync()
    {
        _logger.LogInformation("Bắt đầu lấy dữ liệu cho trang chủ.");

        var heroBanners = await _context.Banners
            .AsNoTracking()
            .Where(b => b.IsActive && b.Type == BannerType.Slide)
            .OrderBy(b => b.OrderIndex)
            .ProjectTo<BannerViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var featuredBrands = await _context.Brands
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .Take(6)
            .ProjectTo<BrandLogoViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var featuredProducts = await _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive && p.IsFeatured && p.Status == PublishStatus.Published)
            .Include(p => p.Brand)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .Take(8)
            .ProjectTo<ProductCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var latestArticles = await _context.Articles
            .AsNoTracking()
            .Where(a => a.Status == PublishStatus.Published && a.PublishedAt <= DateTime.Now)
            .Include(a => a.Category)
            .OrderByDescending(a => a.PublishedAt)
            .Take(3)
            .ProjectTo<ArticleCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        _logger.LogInformation(
            "Lấy dữ liệu trang chủ thành công với {BannerCount} banners, {BrandCount} thương hiệu, {ProductCount} sản phẩm, {ArticleCount} bài viết.",
            heroBanners.Count,
            featuredBrands.Count,
            featuredProducts.Count,
            latestArticles.Count);

        return new HomeViewModel
        {
            HeroBanners = heroBanners,
            FeaturedBrands = featuredBrands,
            FeaturedProducts = featuredProducts,
            LatestArticles = latestArticles
        };
    }
}