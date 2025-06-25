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
public class ClientSearchService : IClientSearchService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClientSearchService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClientSearchViewModel> SearchAsync(string query)
    {
        var viewModel = new ClientSearchViewModel { Query = query };

        if (string.IsNullOrWhiteSpace(query))
        {
            return viewModel; // Trả về rỗng nếu không có từ khóa
        }

        var lowerQuery = query.Trim().ToLower();
        const int resultLimit = 12; // Giới hạn số kết quả cho mỗi loại

        // Tìm kiếm sản phẩm
        var productTask = _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive && p.Status == PublishStatus.Published)
            .OrderByDescending(p => p.IsFeatured)
            .Take(resultLimit)
            .ProjectTo<ProductCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        // Tìm kiếm bài viết
        var articleTask = _context.Articles
            .AsNoTracking()
            .Where(a => a.Status == PublishStatus.Published && a.PublishedAt <= DateTime.Now &&
                        (a.Title.ToLower().Contains(lowerQuery) ||
                         (a.Summary != null && a.Summary.ToLower().Contains(lowerQuery))))
            .OrderByDescending(a => a.PublishedAt)
            .Take(resultLimit)
            .ProjectTo<ArticleCardViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        // Chạy song song 2 câu truy vấn để tăng hiệu năng
        await Task.WhenAll(productTask, articleTask);

        viewModel.ProductResults = await productTask;
        viewModel.ArticleResults = await articleTask;

        return viewModel;
    }
}